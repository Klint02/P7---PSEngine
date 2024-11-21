using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using CloudSearcher;

namespace CloudSearcher

{
    // This class will be responsible for conducting a boolean search on the inverted index
    public class BooleanSearch
    {
        private InvertedIndex _invertedIndex;

        public BooleanSearch(InvertedIndex invertedIndex)
        {
            _invertedIndex = invertedIndex;
        }

        // This method will process a search query and return a list of search terms (tokens)
        public List<string> ProcessSearchQuery(string searchTerm)
        {
            // Split the search query into terms
            var searchTerms = Regex.Split(searchTerm.ToLower(), @"\W+")
                .Where(term => !string.IsNullOrEmpty(term))
                .ToList();

            return searchTerms;
        }

        
        // This method will take a query string and return a list of document IDs that contain the query terms
        public SearchResult BoolSearch(string searchTerm)
        {
            // Process the search query
            var searchTerms = ProcessSearchQuery(searchTerm); 
            // Create a list to store the search results
            // The list should contain the document IDs
            // This should be a list of dictionaries, where each dictionary contains the document ID and the filename
            var searchResults = new SearchResult { SearchTerm = searchTerm };

            // For each term in the search query, get the TermInfo from the inverted index
            foreach (var term in searchTerms)
            {
                // Get the TermData object for the term
                if (!_invertedIndex.invertedIndex.ContainsKey(term))
                {
                    continue; // If the term is not in the inverted index, skip to the next term
                }

                var termData = _invertedIndex.invertedIndex[term];

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

        // This method will calculate the total number of results for a single search term
        // If the term is in multiple documents, the total number of results should be the 
        // sum of the term frequency in each document

        public int CalculateTotalResults(string searchTerm)
        {
            // Determine the number of documents that contain the search term
            var termDocFreq = _invertedIndex.GetDocumentFrequency(searchTerm);
            var termInfo = _invertedIndex.GetTermData(searchTerm);
            var termDocuments = _invertedIndex.invertedIndex[searchTerm].Documents;

            // If the term is not in the inverted index, return 0
            if (termDocFreq == 0)
            {
                return 0;
            }

            // If the term is in one document, return the frequency of the term in that document
            if (termDocFreq == 1)
            {
                return _invertedIndex.invertedIndex[searchTerm].TotalTermFrequency;

            }
            // If the term is in multiple documents, return the sum of the term frequency in each document
            else
            {
                return _invertedIndex.invertedIndex[searchTerm].TotalTermFrequency;
            }
        }

        // This method will display the search results
        public void DisplaySearchResults(SearchResult searchResults)
        {
            Console.WriteLine($"Search term: {searchResults.SearchTerm}");
            Console.WriteLine($"Total results: {searchResults.TotalResults}");
            Console.WriteLine("Search results:");
            foreach (var result in searchResults.SearchResults)
            {
                Console.WriteLine($"Document ID: {result.DocID}, Filename: {result.Filename}, Term Frequency: {result.TermFrequency}");
            }
        } 

    }


    // This class is used to store the results of a search
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