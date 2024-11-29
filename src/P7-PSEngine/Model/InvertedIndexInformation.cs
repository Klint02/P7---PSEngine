namespace P7_PSEngine.Model
{
    public class InvertedIndexInformation
    {
        public InvertedIndexInformation()
        {
            
        }
        
        public string Word { get; set; }
        public int UserId { get; set; }
        public int FileFrequency { get; set; }
        public int TotalWordFrequency { get; set; }
        public ICollection<WordInformation> WordInformations { get; set; }
        public User User { get; set; }
    }
}
