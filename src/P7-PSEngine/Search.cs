using CloudFileIndexer;
using System.Text.RegularExpressions;
//using Microsoft.EntityFrameworkCore.Migrations.Operations;

// namespace CloudSearcher

// {
//     // This class will be responsible for conducting a boolean search on the inverted index
//     public class BooleanSearch
//     {
//         private InvertedIndex _invertedIndex;

//         public BooleanSearch(InvertedIndex invertedIndex)
//         {
//             _invertedIndex = invertedIndex;
//         }

        // This method will take a query string and return a list of document IDs that contain the query terms
        public SearchResult BSearch(string searchTerm)
        {
            // Split the search query into terms
            var searchTerms = Regex.Split(searchTerm.ToLower(), @"\W+")
                .Where(term => !string.IsNullOrEmpty(term))
                .ToList();
            Console.WriteLine("Running search procedure");
            Console.WriteLine("Search terms: " + string.Join(", ", searchTerms));
            Console.WriteLine(_invertedIndex.tester);
            Console.WriteLine("Test af GetTermInfo:" + _invertedIndex.invertedIndex.Count().ToString());
            _invertedIndex.DisplayIndex();


            // Create a list to store the search results
            // The list should contain the document IDs as well as the filenames
            // This should be a list of dictionaries, where each dictionary contains the document ID and the filename
            var finalResults = new Dictionary<string, TermInfo>();

            //var searchResults = new HashSet<(string DocID, string Filename)>();
            // For each term in the search query, get the TermInfo from the inverted index
            foreach (var term in searchTerms)
            {
                Dictionary<string, TermInfo> termInfo = _invertedIndex.GetTermInfo(term);
                Console.WriteLine("Term: " + term);
                Console.WriteLine("TermInfo: " + termInfo.ToString());
                // If the term is not in the inverted index, continue to the next term
                if (termInfo != null)
                {
                    if (finalResults == null)
                    {
                        finalResults = new Dictionary<string, TermInfo>(termInfo);
                    }
                    else
                    {
                        // Return all documents that contain one or more of the search terms
                        finalResults = finalResults
                            .Where(entry => termInfo.ContainsKey(entry.Key))
                            .ToDictionary(entry => entry.Key, entry => entry.Value);

                    }
                    var searchResults = finalResults?.Select(kv => new Dictionary<string, string>
                    {
                        {"DocID", kv.Key},
                        {"Filename", _invertedIndex.GetFileName(kv.Key)}
                    }).ToList() ?? new List<Dictionary<string, string>>();
                    // write the search results to the console
                    foreach (var result in searchResults)
                    {
                        Console.WriteLine("DocID: " + result["DocID"] + ", Filename: " + result["Filename"]);
                    }

                    // return the search results
                    return new SearchResult
                    {
                        TotalResults = searchResults.Count,
                        SearchResults = searchResults
                    };
                }
            }
            return new SearchResult
            {
                TotalResults = 0,
                SearchResults = new List<Dictionary<string, string>>()
            };
        }
    }
    public class SearchResult
    {
        // This class will be used to store the results of a search
        // It should contain a dictionary of document IDs and filenames as well as the total number of results

        public int TotalResults { get; set; }
        public List<Dictionary<string, string>> SearchResults { get; set; } = new List<Dictionary<string, string>>();

        /*        public void AddResult(string docID, string filename)
                {
                    SearchResults.Add(new Dictionary<string, string> {{"DocID", docID}, {"Filename", filename}});
                }
        */
    }

}



