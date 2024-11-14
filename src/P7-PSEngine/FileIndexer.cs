using Newtonsoft.Json;
using System.Text.RegularExpressions;

namespace CloudFileIndexer

{
    public class InvertedIndex
    {
        // Dictionary to store the inverted index
        // Key: term, Value: dictionary of document IDs and term info
        public Dictionary<string, Dictionary<string, TermInfo>> invertedIndex = new Dictionary<string, Dictionary<string, TermInfo>>();
        // Dictionary to store the document frequency of each term
        private Dictionary<string, int> documentFrequency = new Dictionary<string, int>();
        // Dictionary to store metadata of each document
        private Dictionary<string, DocumentMetadata> documentMetadata = new Dictionary<string, DocumentMetadata>();

        public string tester = "";

        // Method to add Metadata
        public void AddMetadata(string docId, string fileName)
        {
            if (!documentMetadata.ContainsKey(docId))
            {
                documentMetadata[docId] = new DocumentMetadata
                {
                    DocID = docId,
                    Filename = fileName
                };
            }
        }

        public string GetFileName(string docId)
        {
            if (documentMetadata.TryGetValue(docId, out var metadata))
            {
                return metadata.Filename;
            }
            else
            {
                return string.Empty;
            }
        }

        public void AddTerm(string term, string docId, int position)
        {
            term = term.ToLower();

            if (!invertedIndex.ContainsKey(term))
            {
                invertedIndex[term] = new Dictionary<string, TermInfo>();
                documentFrequency[term] = 0;
            }

            if (!invertedIndex.ContainsKey(docId))
            {
                invertedIndex[term][docId] = new TermInfo();
                documentFrequency[term]++;
            }

            invertedIndex[term][docId].TermFrequency++;
            invertedIndex[term][docId].Positions.Add(position);
        }


        public void IndexDocument(string docId, string content)
        {
            // Tokenize content and track position
            var tokens = Regex.Split(content.ToLower(), @"\W+"); // Splitting by non-word characters
            for (int i = 0; i < tokens.Length; i++)
            {
                string token = tokens[i];
                if (!string.IsNullOrEmpty(token))
                {
                    AddTerm(token, docId, i); // Use the position in the document as `i`
                }
            }
            AddMetadata(docId, content); // Add metadata for the document
        }

        public Dictionary<string, TermInfo> GetTermInfo(string term)
        {
            if (invertedIndex.TryGetValue(term, out var termInfo))
            {
                var termInfoCopy = new Dictionary<string, TermInfo>(termInfo);
                return termInfoCopy;
            }
            else
            {
                return new Dictionary<string, TermInfo>();
            }
        }

        // Method to display the inverted index
        public void DisplayIndex()
        {
            Console.WriteLine("Displaying index:");
            foreach (var termEntry in invertedIndex)
            {

                Console.WriteLine($"Term: {termEntry.Key}, Total Frequency: {GetDocumentFrequency(termEntry.Key)}");
                foreach (var docEntry in termEntry.Value)
                {
                    var info = docEntry.Value;
                    Console.WriteLine($"\tDocId: {docEntry.Key}, TF: {info.TermFrequency}, Positions: {string.Join(", ", info.Positions)}");
                    Console.WriteLine($"\t\tFilename: {GetFileName(docEntry.Key)}");
                }
            }
        }

        public string GetIndexData()
        {
            // Sort positions in each TermInfo for consistent ordering
            foreach (var termEntry in invertedIndex)
            {
                foreach (var docEntry in termEntry.Value)
                {
                    docEntry.Value.Positions.Sort();
                }
            }

            // Serialize the entire invertedIndex to JSON
            return JsonConvert.SerializeObject(invertedIndex, Formatting.Indented);
        }

        public int GetDocumentFrequency(string term)
        {
            term = term.ToLower();
            return documentFrequency.ContainsKey(term) ? documentFrequency[term] : 0;
        }
    }

    public class TermInfo
    {
        public int TermFrequency { get; set; } = 0;
        public List<int> Positions { get; set; } = new List<int>();

        public string FileName { get; set; } = "";

        public override string ToString()
        {
            return $"FileName: {FileName}, Frequency: {TermFrequency}, Positions: {string.Join(", ", Positions)}";
        }
    }
}

