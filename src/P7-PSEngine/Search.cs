using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using CloudFileIndexer;
using Microsoft.VisualBasic;
using System.Reflection.Metadata.Ecma335;

namespace CloudSearcher

{
    // This class will be responsible for conducting a boolean search on the inverted index
    public class BooleanSearch(InvertedIndex invertedIndex)
    {
        private readonly InvertedIndex _invertedIndex = invertedIndex;

        // This method will take a query string and return a list of document IDs that contain the query terms
        public SearchResult BSearch(string SearchTerm)
        {
            // Store the inverted index
            // This will be used to get the term info for each term in the search query



            // Split the search query into terms
            //Dictionary<string, string> finalResults = null;
            
            var searchTerms = Regex.Split(SearchTerm.ToLower(), @"\W+"); // Splitting by non-word characters
            
            // Create a list to store the search results
            // The list should contain the document IDs as well as the filenames
            // This should be a list of dictionaries, where each dictionary contains the document ID and the filename

            var searchResults = new List<Dictionary<string, string>>();

            // For each term in the search query, get the TermInfo from the inverted index
            foreach (var term in searchTerms)
            {
                var termInfo = _invertedIndex.GetTermInfo(term);
            
            
                // If the term is not in the inverted index, continue to the next term
                if (termInfo == null)
                {
                    continue;
                }

            var currentresults = termInfo.ToDictionary(entry => entry.Key, entry => entry.Value);

            

            finalResults = finalResults == null
                ? currentresults
                : finalResults.Keys.Intersect(currentresults.Keys)
                    .ToDictionary(docID => docID, docID => currentresults[docID]);
        }

        var searchResults = finalResults.Select(kv => new Dictionary<string, string> {{"DocID", kv.Key}, {"Filename", kv.Value}}).ToList();

        return new SearchResult
        {
            TotalResults = searchResults.Count ?? 0,
            SearchResults = searchResults ?? new List<Dictionary<string, string>>()
        };
    }
}

    public class SearchResult
    {
        // This class will be used to store the results of a search
        // It should contain a dictionary of document IDs and filenames as well as the total number of results

        public int TotalResults { get; set; }
        public List<Dictionary<string, string>> SearchResults { get; set; }
 
    }




}
