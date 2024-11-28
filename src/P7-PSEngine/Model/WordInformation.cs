using System.ComponentModel.DataAnnotations;

namespace P7_PSEngine.Model
{
    public class WordInformation
    {
        public string Word { get; set; }
        public string FileID { get; set; }
        public int UserId { get; set; }
        public int WordFrequency { get; set; }

        public InvertedIndexInformation InvertedIndex { get; set; }
        public FileInformation FileInformation { get; set; }
        public User User { get; set; }
        public WordInformation(string word) 
        {
            Word = word;
        }
    }
}
