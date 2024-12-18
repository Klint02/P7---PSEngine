public class SearchResultsDTO
{

    // It should contain a dictionary of document IDs and filenames as well as the total number of results
    public string SearchTerm { get; set; } = "";
    // The total number of results for the search term including multiple entries in the same document
    public int TotalResults { get; set; } = 0;

    // List of dictionaries to store search results
    // Each dictionary contains the document ID, filename, and term frequency
    public List<SearchResultItem> SearchResults { get; set; } = new List<SearchResultItem>();
}

public class SearchResultItem
{
    public string fileId { get; set; } = "";
    public string fileName { get; set; } = "";
    public string path { get; set; } = "";

    public DateTime dateCreated { get; set; } = DateTime.Now;
    public int termFrequency { get; set; } = 0;
}