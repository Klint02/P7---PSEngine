using Microsoft.EntityFrameworkCore;
using P7_PSEngine.Data;
using P7_PSEngine.Model;
using System.Text.RegularExpressions;
using System.Diagnostics;

namespace P7_PSEngine.Services
{
    public interface ISearchService
    {
        Task<IEnumerable<FileInformation>> SearchFiles(IEnumerable<string> search);
        Task<IEnumerable<FileInformation>> GetALlFilesWithIndex();
        List<string> ProcessSearchQuery(string searchTerm);
        Task<SearchResult> BoolSearch(string searchTerm, User user, SearchDetailsDTO searchDetails);
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

        public async Task<SearchResult> BoolSearch(string searchTerm, User user, SearchDetailsDTO searchDetails)
        {
            
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            // Process the search query
            Console.WriteLine($"Searching for: {searchTerm}");
            var searchTerms = ProcessSearchQuery(searchTerm);
            //Console.WriteLine("Search terms: ", searchTerms);
            Console.WriteLine($"Search terms af ProcessSearchQuery: {string.Join(", ", searchTerms)}");

            // Get all documents and calculate total number of documents
            var allFiles = await GetALlFilesWithIndex();

            // Apply filters to the files searched through
            if (searchDetails.folderOption)
            {
                allFiles = allFiles.Where(f => f.FileType == "Folder");
            }

            if (searchDetails.startDate != null)
            {
                allFiles = allFiles.Where(f => f.CreationDate >= searchDetails.startDate);
            }

            if (searchDetails.endDate != null)
            {
                allFiles = allFiles.Where(f => f.CreationDate <= searchDetails.endDate);
            }

            if (searchDetails.imageOption)
            {
                allFiles = allFiles.Where(f =>
                    f.FileName.EndsWith(".jpg", StringComparison.OrdinalIgnoreCase) || 
                    f.FileName.EndsWith(".jpeg", StringComparison.OrdinalIgnoreCase) ||
                    f.FileName.EndsWith(".png", StringComparison.OrdinalIgnoreCase) ||
                    f.FileName.EndsWith(".gif", StringComparison.OrdinalIgnoreCase));
            }

            if (searchDetails.docOption)
            {
                allFiles = allFiles.Where(f =>
                    f.FileName.EndsWith(".doc", StringComparison.OrdinalIgnoreCase) ||
                    f.FileName.EndsWith(".docx", StringComparison.OrdinalIgnoreCase) ||
                    f.FileName.EndsWith(".pdf", StringComparison.OrdinalIgnoreCase) ||
                    f.FileName.EndsWith(".txt", StringComparison.OrdinalIgnoreCase));
            }


            int totalDocuments = allFiles.Count();
            // Create a list to store the search results
            // var searchResults = new SearchResult { SearchTerm = searchTerm };

            var termData = await FindTerm(searchTerms, user);
            Console.WriteLine($"Found {termData.Count()} terms in the index");

            // Initialize result map for cosine similarity calculation
            var documentVectors = new Dictionary<string, Dictionary<string, double>>();
            var queryVector = new Dictionary<string, double>();

            // Build the query vector
            foreach (var term in searchTerms)
            {
                var termEntry = termData.FirstOrDefault(t => t.Term == term);
                if (termEntry != null)
                {
                    double idf = Math.Log((double)totalDocuments / (1 + termEntry.TermInformations.Count()));
                    queryVector[term] = idf;
                }
                else
                {
                    queryVector[term] = 0;
                }
            }

            // Populate document vectors with TF-IDF scores
            foreach (var termEntry in termData)
            {
                foreach (var termInfo in termEntry.TermInformations)
                {
                    if (!documentVectors.ContainsKey(termInfo.FileId))
                    {
                        documentVectors[termInfo.FileId] = new Dictionary<string, double>();
                    }

                    double tfidf = CalculateTFIDF(
                        termInfo.TermFrequency, 
                        termEntry.FileFrequency, 
                        totalDocuments);
                    documentVectors[termInfo.FileId][termEntry.Term] = tfidf;
                }
            }

            // Compute cosine similarity scores
            var documentScores = new Dictionary<string, double>();
            foreach (var docId in documentVectors.Keys)
            {
                double dotProduct = 0, queryMagnitude = 0, docMagnitude = 0;

                foreach (var term in searchTerms)
                {
                    double queryWeight = queryVector.ContainsKey(term) ? queryVector[term] : 0;
                    double docWeight = documentVectors[docId].ContainsKey(term) ? documentVectors[docId][term] : 0;

                    dotProduct += queryWeight * docWeight;
                    queryMagnitude += Math.Pow(queryWeight, 2);
                    docMagnitude += Math.Pow(docWeight, 2);
                }

                queryMagnitude = Math.Sqrt(queryMagnitude);
                docMagnitude = Math.Sqrt(docMagnitude);

                if (queryMagnitude > 0 && docMagnitude > 0)
                {
                    documentScores[docId] = dotProduct / (queryMagnitude * docMagnitude);
                }
            }

            // Sort the search results by cosine similarity score
            var sortedResults = documentScores.OrderByDescending(kvp => kvp.Value)
                .ToList();

            // Build the SearchResult 
            SearchResult searchResults = new SearchResult(searchTerm);
            foreach (var (docId, score) in sortedResults)
            {
                var file = allFiles.FirstOrDefault(f => f.FileId == docId);
                if (file != null)
                {
                    var termFrequencies = termData
                    .Where(td => td.TermInformations.Any(ti => ti.FileId == docId))
                    .ToDictionary(td => td.Term, td => td.TermInformations.First(ti => ti.FileId == docId).TermFrequency);
                    
                    searchResults.AddSearchResult
                    (
                        docId,
                        file.FileName,
                        file.FilePath,
                        file.CreationDate,
                        termFrequency: termFrequencies.Values.Sum(),
                        term: string.Join(", ", documentVectors[docId].Keys)
                    );

                    // Add a similarity score to the search result
                    var resultItem = searchResults.SearchResults.Last();
                    resultItem.similarityScore = Math.Round(score * 100, 2);
            }
        }
        searchResults.TotalResults = searchResults.SearchResults.Count;

        stopwatch.Stop();
        Console.WriteLine($"Time taken: {stopwatch.ElapsedMilliseconds} ms");
        Console.WriteLine($"Time taken (seconds): {stopwatch.Elapsed.TotalSeconds}");
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
            //Console.WriteLine($"Searching for term: {string.Join(", ", term)}");
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
    

        // Method to calculate the term frequency-inverse document frequency (TF-IDF) score
        private double CalculateTFIDF(int termFrequency, int FileFrequency, int totalDocuments)
        {
        // Calculate the term frequency-inverse document frequency (TF-IDF) score
        // TF-IDF = TF * IDF
        // TF = term frequency in document / total terms in document
        // IDF = log(total documents / documents with term)
        // TF-IDF = term frequency * log(total documents / documents with term
            if (FileFrequency == 0 || totalDocuments == 0)
            {
                return 0;
            }

            double tf = (double)termFrequency;
            double idf = Math.Log((double)totalDocuments / (1 + FileFrequency));
            return tf * idf;
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
        public double similarityScore { get; set; } = 0;
    }
}
