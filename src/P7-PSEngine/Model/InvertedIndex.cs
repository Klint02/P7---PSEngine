namespace P7_PSEngine.Model
{
    public class InvertedIndex
    {
        public InvertedIndex()
        {
            TermDocuments = new List<TermInformation>();
        }
        
        public string Term { get; set; }
        public int UserId { get; set; }
        public int DocumentFrequency { get; set; }
        public int TotalTermFrequency { get; set; }
        public ICollection<TermInformation> TermDocuments { get; set; }
        public User User { get; set; }
    }
}
