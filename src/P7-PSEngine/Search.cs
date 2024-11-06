using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using CloudFileIndexer;

namespace CloudSearcher

{
    // This class will be responsible for conducting a boolean search on the inverted index
    public class BooleanSearch
    {
        private InvertedIndex invertedIndex;
        
        public BooleanSearch(InvertedIndex invertedIndex)
        {
            this.invertedIndex = invertedIndex;
        }
        
        // This method will take a query string and return a list of document IDs that contain the query terms
        public SearchResult BooleanSearch(string SearchTerm, InvertedIndex invertedIndex)
        {
            _invertedIndex = invertedIndex;
            // Split the search query into terms
            var searchTerms = Regex.Split(SearchTerm.ToLower(), @"\W+"); // Splitting by non-word characters
            
            // Create a list to store the search results
            // The list should contain the document IDs as well as the filenames
            // This should be a list of dictionaries, where each dictionary contains the document ID and the filename

            var searchResults = new List<Dictionary<string, string>>();

            // For each term in the search query, get the TermInfo from the inverted index
            foreach (var term in searchTerms)
            {
                var termInfo = _invertedIndex.GetTermInfo(term);
                if (termInfo != null)
                {
                    searchResults.AddRange(termInfo.Keys);
                }
            }

            var result = new SearchResult();

            result.DocumentIds = searchResults;

            result.TotalResults = searchResults.Count;

            return result;

        }


    }



    public class SearchResult
    {
        // This class will be used to store the results of a search
        // It should contain a dictionary of document IDs and filenames as well as the total number of results

        public int TotalResults { get; set; }
        public List<Dictionary<string, string>> SearchResult { get; set; }
 
    }




}
