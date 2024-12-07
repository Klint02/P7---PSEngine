using Microsoft.EntityFrameworkCore;
using P7_PSEngine.Data;
using P7_PSEngine.Model;
using System.Text.RegularExpressions;
using P7_PSEngine.Repositories;

namespace P7_PSEngine.Services
{
    public interface ISearchService
    {
        Task<IEnumerable<FileInformation>> SearchFiles(IEnumerable<string> search);
        Task<IEnumerable<FileInformation>> GetALlFilesWithIndex();
        Task<IEnumerable<string>> ProcessSearchQuery(string searchTerm);
        Task<SearchResult> BoolSearch(string searchTerm);
    }

    public class SearchService : ISearchService
    {
        private readonly PSengineDB _db;

        public SearchService(PSengineDB db)
        {
            _db = db;
        }

        public async Task<IEnumerable<string>> ProcessSearchQuery(string searchTerm)
        {
            // Split the search query into terms
            IEnumerable<string> searchTerms = Regex.Split(searchTerm.ToLower(), @"\W+")
                .Where(term => !string.IsNullOrEmpty(term))
                .ToList();

            return searchTerms;
        }

        public async Task<SearchResult> BoolSearch(string searchTerm)
        {
            // For testing purposes, the user ID is hardcoded to 1
            int userId = 1;
            
            // Process the search query
            var searchTerms = await ProcessSearchQuery(searchTerm); 

            // Create a list to store the search results
            var searchResults = new SearchResult { SearchTerm = searchTerm };

            // Find the search term in the inverted index
            foreach (var term in searchTerms)
            {
                Console.WriteLine($"Searching for term: {term}");
                var termData = await FindTerm(new List<string> { term }, userId);

                // Iterate through the search results and add them to the list
                if (termData == null || !termData.Any())
                {
                    Console.WriteLine($"No search results found for {term}");
                    continue;
                }
                foreach (var doc in termData)
                {
                    foreach (var index in doc.TermInformations)
                    {
                        index.InvertedIndex = null;
                        searchResults.SearchResults.Add(new SearchResultItem
                        {
                            fileId = index.FileId,
                            fileName = "",
                            termFrequency = index.TermFrequency,
                        });
                        
                        // Increment the total number of search results
                        searchResults.TotalResults += index.TermFrequency;
                    }
                }
            }

            // Calculate the total number of search results
            Console.WriteLine($"Search results for {searchTerm}: {searchResults.TotalResults}");
            // Print all the files that contain the search term
            foreach (var result in searchResults.SearchResults)
            {
                Console.WriteLine($"File ID: {result.fileId}, Filename: {result.fileName}, Term Frequency: {result.termFrequency}");
            }
            return searchResults;
}
        public async Task<IEnumerable<FileInformation>> SearchFiles(IEnumerable<string> search)
        {
            List<FileInformation> files = await _db.FileInformation.Include(p => p.TermFiles.Where(p => search.Contains(p.Term)))
                .Where(p => p.TermFiles.Any(index => search.Contains(index.Term)))
                .AsNoTracking()
                .ToListAsync();

            return files;
        }

        public async Task<IEnumerable<FileInformation>> GetALlFilesWithIndex()
        {
            List<FileInformation> files = await _db.FileInformation.Include(p => p.TermFiles).AsNoTracking().ToListAsync();

            return files;
        }

        public async Task<List<InvertedIndex>> FindTerm(IEnumerable<string> term, int userId)
        {
/*            var kage = await _db.TermInformations.Include(p => p.FileInformation).Include(p => p.InvertedIndex)
                .Where(p => term.Contains(p.Term) && p.UserId == userId)
                .ToListAsync();*/

            List<InvertedIndex> invertedIndices = await _db.InvertedIndex.Include(p => p.TermInformations.Where(p => term.Contains(p.Term)))
                .ThenInclude(p => p.FileInformation)
                .Where(p => p.TermInformations.Any(index => term.Contains(index.Term)))
                .Where(p => p.UserId == userId)
                .ToListAsync();

            return invertedIndices;
        }
    }

     public class SearchResult
    {

        // It should contain a dictionary of document IDs and filenames as well as the total number of results
        public string SearchTerm { get; set; } = "";
        // The total number of results for the search term including multiple entries in the same document
        public int TotalResults { get; set; } = 0;
        
        // List of dictionaries to store search results
        // Each dictionary contains the document ID, filename, and term frequency
        public List<SearchResultItem> SearchResults { get; set; } = new List<SearchResultItem>();

        public SearchResult()
        {
            TotalResults = 0;
            SearchResults = new List<SearchResultItem>();
        }

        // Method to add a search result
        public void AddSearchResult(string fileid, string filename, int termFrequency)
        {
            var result = new SearchResultItem
                {
                    fileId = fileid,
                    fileName = filename,
                    termFrequency = termFrequency
                };
            Console.WriteLine($"Adding search result: {result.fileId}, {result.fileName}, {result.termFrequency}");
            SearchResults.Add(result);
        }
    }

    // This class is used to store the results of a search
    public class SearchResultItem
    {
        public string fileId { get; set; } = "";
        public string fileName { get; set; } = "";
        public string path { get; set; } = "";

        public DateTime dateCreated { get; set; } = DateTime.Now;
        public int termFrequency { get; set; } = 0;
    }
}
