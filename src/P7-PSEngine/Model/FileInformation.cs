namespace P7_PSEngine.Model
{
    public class FileInformation
    {
        public string FileId { get; set; }
        public string FileName { get; set; }

        public User User { get; set; }

        public string FilePath { get; set; }

        public string FileType { get; set; }
        public ICollection<TermInformation> TermFiles { get; set; }
        public DateTime ChangedDate { get; set; }
        public DateTime CreationDate { get; set; }

        public int UserId { get; set; }

        public CloudService SID { get; set; }
    }
}
