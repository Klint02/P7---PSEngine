using System.Reflection.Metadata;

namespace P7_PSEngine.Model
{
    public class InvertedIndexClass
    {
    }

    public class FileList
    {
        public string NextPageToken { get; set; } = string.Empty;
        public string Kind { get; set; } = string.Empty;
        public bool IncompleteSearch { get; set; } = false;
        public List<FileData> Files { get; set; } = new List<FileData>();
    }

    public class FileData
    {
        public string Kind { get; set; } = string.Empty;
        public string MimeType { get; set; } = string.Empty;
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
    }

    public class SearchResult
    {
        // This class will be used to store the results of a search
        // It should contain a dictionary of document IDs and filenames as well as the total number of results

        public int TotalResults { get; set; }
        public List<Dictionary<string, string>> SearchResults { get; set; } = new ();

        public void test()
        {
            
        }
            

            /*        public void AddResult(string docID, string filename)
                    {
                        SearchResults.Add(new Dictionary<string, string> {{"DocID", docID}, {"Filename", filename}});
                    }
            */
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

    public class DocumentMetadata
    {
        public string DocID { get; set; } = string.Empty;
        public string Filename { get; set; } = string.Empty;
        public string Path { get; set; } = string.Empty;
        public string MimeType { get; set; } = string.Empty;
        public long Size { get; set; } = 0;
        public DateTime LastModified { get; set; } = DateTime.Now;
    }
}
