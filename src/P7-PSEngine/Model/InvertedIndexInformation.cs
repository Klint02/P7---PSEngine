using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace P7_PSEngine.Model
{
    public class InvertedIndexInformation
    {
        public InvertedIndexInformation(string word)
        {
            Word = word;
        }
        
        public string Word { get; set; }
        public int UserId { get; set; }
        public int FileFrequency { get; set; }
        public int TotalWordFrequency { get; set; }
        public ICollection<WordInformation> WordInformations { get; set; }
        public User User { get; set; }
    }
}
