using System.ComponentModel.DataAnnotations;

namespace P7_PSEngine.Model
{
    public class FileInformation
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string FileId { get; set; }
        [Required]
        public string FileName { get; set; }
        [Required]
        public string FilePath { get; set; }
        [Required]
        public string FileType { get; set; }
        public ICollection<IndexInformation> IndexInformations { get; set; }
        //public DateTime ChangedDate { get; set; }
        //public DateTime CreationDate { get; set; }
        //Service ID, but the model for service is not created yet

        public FileInformation()
        {
            IndexInformations = [];
        }
    }
}
