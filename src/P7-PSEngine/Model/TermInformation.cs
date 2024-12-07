namespace P7_PSEngine.Model
{
    public class TermInformation
    {
        public string Term { get; set; }
        public string FileId { get; set; }
        public int UserId { get; set; }
        public int TermFrequency { get; set; }

        public InvertedIndex InvertedIndex { get; set; }
        public FileInformation FileInformation { get; set; }
        public User User { get; set; }
        public TermInformation()
        {

        }
    }
}
