using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace P7_PSEngine.Model
{
    public class FileInformation
    {
        [Key]
        public int Id { get; set; }
        //[Required]
        //public string FileId { get; set; }
        [Required]
        public string FileName { get; set; }
        [Required]
        public string FilePath { get; set; }
        [Required]
        public string FileType { get; set; }
        public ICollection<IndexInformation> IndexInformations { get; set; }
        public DateTime ChangedDate { get; set; }
        public DateTime CreationDate { get; set; }
        //Service ID, but the model for service is not created yet

        [Required, ForeignKey("User")]
        public User UID { get; set; }

        [Required, ForeignKey("CloudService")]
        public CloudService SID { get; set; }
        public FileInformation()
        {
            IndexInformations = [];
        }
    }
}
