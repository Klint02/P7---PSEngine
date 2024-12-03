namespace P7_PSEngine.Model
{
    public class DocumentInformation
    {
        public string DocId { get; set; }
        public string DocumentName { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        //[Required]
        //public string FilePath { get; set; }
        //[Required]
        //public string FileType { get; set; }
        public ICollection<TermInformation> TermDocuments { get; set; }
        //public DateTime ChangedDate { get; set; }
        //public DateTime CreationDate { get; set; }
        //Service ID, but the model for service is not created yet

    }
}
