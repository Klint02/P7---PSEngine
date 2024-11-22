using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace P7_PSEngine.Model
{
    public class IndexInformation
    {
        public IndexInformation()
        {

        }
        public IndexInformation(string word, int position)
        {
            Word = word;
            Positions = position;
        }
        [Key]
        public int Id { get; set; }
        [Required]
        public string Word { get; set; }
        //[Required]
        //public float Frequency { get; set; }
        [Required]
        public int Positions { get; set; }
        public FileInformation FileInformation { get; set; }
    }
}
