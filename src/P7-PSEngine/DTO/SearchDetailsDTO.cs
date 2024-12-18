public class SearchDetailsDTO
{
    public string searchwords { get; set; } = "";
    public bool filenameOption { get; set; }
    public bool contentOption { get; set; }
    public bool mailOption { get; set; }
    public bool docOption { get; set; }
    public bool folderOption { get; set; }
    public bool imageOption { get; set; }
    public bool miscOption { get; set; }
    public DateTime? startDate { get; set; }
    public DateTime? endDate { get; set; }
}