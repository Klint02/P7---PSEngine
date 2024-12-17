using Microsoft.EntityFrameworkCore;
using P7_PSEngine.Data;
using P7_PSEngine.Model;
using System.Text.RegularExpressions;

namespace P7_PSEngine.Services
{
    public interface ISearchService
    {
        Task<IEnumerable<FileInformation>> SearchFiles(IEnumerable<string> search);
        Task<IEnumerable<FileInformation>> GetALlFilesWithIndex();
        List<string> ProcessSearchQuery(string searchTerm);
        Task<SearchResult> BoolSearch(string searchTerm, User user);
    }

    //TODO (djb) If serached with more than 1 word return more inverted terms
    //TODO (djb) Maybe send invertedIndex with OrderBy, so the files are in a descending order
    public class SearchService : ISearchService
    {
        private readonly PSengineDB _db;

        public SearchService(PSengineDB db)
        {
            _db = db;
        }

        public List<string> ProcessSearchQuery(string searchTerm)
        {
            // Split the search query into terms
            var searchTerms = Regex.Split(searchTerm.ToLower(), @"[\s_\W]+")
                .Where(term => !string.IsNullOrEmpty(term))
                .ToList();
            Console.WriteLine($"ProcessSearchQuery has Search terms: {string.Join(", ", searchTerms)}");
            return searchTerms;
        }

        public async Task<SearchResult> BoolSearch(string searchTerm, User user)
        {
            // Process the search query
            Console.WriteLine($"Searching for: {searchTerm}");
            var searchTerms = ProcessSearchQuery(searchTerm);
            //Console.WriteLine("Search terms: ", searchTerms);
            Console.WriteLine($"Search terms af ProcessSearchQuery: {string.Join(", ", searchTerms)}");
            // Create a list to store the search results
            // var searchResults = new SearchResult { SearchTerm = searchTerm };
            SearchResult searchResults = new SearchResult(searchTerm);

            var termData = await FindTerm(searchTerms, user);
            Console.WriteLine($"TermData count: {termData.Count()}");

            Console.WriteLine("TJEK:", termData.Count());

            // Dictionary to merge results by file ID
            var resultMap = new Dictionary<string, SearchResultItem>();

            // Find the search term in the inverted index
            foreach (var data in termData)
            {
                foreach (var termInfo in data.TermInformations)
                {
                    string fileId = termInfo.FileInformation.FileId;

                    // If the file ID is not in the search results, add it
                    if (!resultMap.ContainsKey(fileId))
                    {
                        resultMap[fileId] = new SearchResultItem
                        {
                            fileId = fileId,
                            fileName = termInfo.FileInformation.FileName,
                            path = termInfo.FileInformation.FilePath,
                            dateCreated = termInfo.FileInformation.CreationDate,
                            termFrequency = 0,
                            termFrequencies = new Dictionary<string, int>(),
                            matchedTerms = new List<string>()
                        };
                    }

                    // update the term frequency and matched terms
                    resultMap[fileId].termFrequency += termInfo.TermFrequency;
                    if (!resultMap[fileId].termFrequencies.ContainsKey(data.Term))
                    {
                        resultMap[fileId].termFrequencies[data.Term] = 0;
                    }
                    resultMap[fileId].termFrequencies[data.Term] += termInfo.TermFrequency;
                    if (!resultMap[fileId].matchedTerms.Contains(data.Term))
                    {
                        resultMap[fileId].matchedTerms.Add(termInfo.Term);
                    }
                }
            }

            // If no results are found
            if (!resultMap.Any())
            {
                foreach (var term in searchTerms)
                {
                    searchResults.AddSearchResult("0", $"No results found for {term}", "", DateTime.Now, 0, term);
                    Console.WriteLine($"No search results found for {term}");
                }
            }
            else
            {
                searchResults.SearchResults = resultMap.Values
                    .OrderByDescending(result => result.termFrequency)
                    .ToList();
                searchResults.TotalResults = searchResults.SearchResults.Count;
            }

            searchResults.TotalResults = searchResults.SearchResults.Count;

            // Calculate the total number of search results
            searchResults.DisplaySearchResults();
            //            Console.WriteLine($"Search results for {searchTerms}: {searchResults.TotalResults}");
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

        public async Task<List<InvertedIndex>> FindTerm(IEnumerable<string> term, User user)
        {
            Console.WriteLine($"Searching for term: {string.Join(", ", term)}");
            try
            {
                // List<InvertedIndex> invertedIndices = await _db.InvertedIndex.Include(p => p.TermInformations.Where(p => term.Contains(p.Term)))
                //     .ThenInclude(p => p.FileInformation)
                //     .Where(p => p.TermInformations.Any(index => term.Contains(index.Term)))
                //     .Where(p => p.UserId == user.UserId)
                //     .ToListAsync();
                var invertedIndices = await _db.InvertedIndex
                    .Include(p => p.TermInformations)
                    .ThenInclude(p => p.FileInformation)
                    .Where(p => p.TermInformations.Any(index => term.Contains(index.Term)) && p.UserId == user.UserId)
                    .ToListAsync();

                if (invertedIndices == null)
                {
                    Console.WriteLine("No inverted indices found");
                }
                else
                {
                    Console.WriteLine($"Found {invertedIndices.Count()} inverted indices");
                }
                return invertedIndices;
            }
            catch (Exception e)
            {
                Console.WriteLine("Error in FindTerm: ", e.Message);
                return null;
            }
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

        public SearchResult(string searchTerm)
        {
            SearchTerm = searchTerm;
            TotalResults = 0;
            SearchResults = new List<SearchResultItem>();
        }

        // Method to add a search result
        public void AddSearchResult(string fileid, string filename, string path, DateTime date, int termFrequency, string term)
        {
            var result = new SearchResultItem
            {
                fileId = fileid,
                fileName = filename,
                path = path,
                dateCreated = date,
                termFrequency = termFrequency,
                matchedTerms = new List<string> { term }
            };
            //            Console.WriteLine($"Adding search result: {result.fileId}, {result.fileName}, {result.termFrequency}");
            SearchResults.Add(result);
        }

        // Method to display the search results
        public void DisplaySearchResults()
        {
            Console.WriteLine($"Search results for {SearchTerm}:");
            foreach (var result in SearchResults)
            {
                Console.WriteLine($"File ID: {result.fileId}, Filename: {result.fileName}, Path: {result.path}, Date: {result.dateCreated}, Term Frequency: {result.termFrequency}");
            }
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
        public Dictionary<string, int> termFrequencies { get; set; } = new Dictionary<string, int>();
        public List<string> matchedTerms { get; set; } = new List<string>();
    }
}
