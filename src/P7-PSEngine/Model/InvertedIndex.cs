namespace P7_PSEngine.Model
{
    public class InvertedIndex
    {
        public InvertedIndex()
        {
            TermInformations = new List<TermInformation>();
        }

        public string Term { get; set; }
        public int UserId { get; set; }
        public int FileFrequency { get; set; }
        public int TotalTermFrequency { get; set; }
        public ICollection<TermInformation> TermInformations { get; set; }
        public User User { get; set; }
    }
}
