namespace P7_PSEngine.Model
{
    public class FileInformation
    {
        public string FileId { get; set; }
        public string FileName { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        //[Required]
        //public string FilePath { get; set; }
        //[Required]
        //public string FileType { get; set; }
        public ICollection<WordInformation> WordInformations { get; set; }
        //public DateTime ChangedDate { get; set; }
        //public DateTime CreationDate { get; set; }
        //Service ID, but the model for service is not created yet
        
    }
}
