using System.ComponentModel.DataAnnotations;

namespace P7_PSEngine.Model
{
    public class TermInformation
    {
        public string Term { get; set; }
        public string DocID { get; set; }
        public int UserId { get; set; }
        public int TermFrequency { get; set; }

        public InvertedIndex InvertedIndex { get; set; }
        public DocumentInformation DocumentInformation { get; set; }
        public User User { get; set; }
        public TermInformation() 
        {

        }
    }
}
