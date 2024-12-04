using Microsoft.EntityFrameworkCore;
using P7_PSEngine.Data;
using P7_PSEngine.Model;
using System.Text.RegularExpressions;
using P7_PSEngine.Repositories;

namespace P7_PSEngine.Services
{
    public interface ISearchService
    {
        Task<IEnumerable<DocumentInformation>> SearchDocuments(IEnumerable<string> search);
        Task<IEnumerable<DocumentInformation>> GetALlDocumentsWithIndex();
        Task<IEnumerable<string>> ProcessSearchQuery(string searchTerm);
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
            // Process the search query
            var searchTerms = await ProcessSearchQuery(searchTerm); 
            // Create a list to store the search results
            // The list should contain the document IDs
            // This should be a list of dictionaries, where each dictionary contains the document ID and the filename
            var searchResults = new SearchResult { SearchTerm = searchTerm };

            // For each term in the search query, get the TermInfo from the inverted index
            foreach (var term in searchTerms)
            {
                // Get the TermData object for the term
                var termData = await FindTerm(term, userId);

                if (!_db.InvertedIndex.ContainsKey(term))
                {
                    continue; // If the term is not in the inverted index, skip to the next term
                }

                var termData = _db.TermInformations.invertedIndex[term];

                // Increment the total number of results with the total term frequency
                searchResults.TotalResults += termData.TotalTermFrequency;

                foreach (var document in termData.Documents)
                {
                    var docID = document.Key;
                    var documentData = document.Value;

                    // Add the document ID and filename to the search results
                    searchResults.AddSearchResult(
                        docID, 
                        "Filename", 
                        documentData.TermFrequency);
                }
            }
            return searchResults;
        }

        public async Task<IEnumerable<DocumentInformation>> SearchDocuments(IEnumerable<string> search)
        {
            List<DocumentInformation> documents = await _db.DocumentInformation.Include(p => p.TermDocuments.Where(p => search.Contains(p.Term))).Where(p => p.TermDocuments.Any(index => search.Contains(index.Term))).AsNoTracking().ToListAsync();

            return documents;
        }

        public async Task<IEnumerable<DocumentInformation>> GetALlDocumentsWithIndex()
        {
            List<DocumentInformation> documents = await _db.DocumentInformation.Include(p => p.TermDocuments).AsNoTracking().ToListAsync();

            return documents;
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
        public void AddSearchResult(string docId, string filename, int termFrequency)
        {
            var result = new SearchResultItem
                {
                    DocID = docId,
                    Filename = filename,
                    TermFrequency = termFrequency
                };
            SearchResults.Add(result);
        }
    }

    // This class is used to store the results of a search
    public class SearchResultItem
    {
        public string DocID { get; set; } = "";
        public string Filename { get; set; } = "";
        public int TermFrequency { get; set; } = 0;
    }
}
