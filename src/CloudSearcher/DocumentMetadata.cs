using System;
using CloudSearcher;

namespace CloudSearcher
{
    public class DocumentMetadata
    {
        public string DocID { get; set; }
        public string Filename { get; set; } 
        public string Path { get; set; } 
        public string MimeType { get; set; } 
        public long Size { get; set; }
        public DateTime LastModified { get; set; }


         public DocumentMetadata()
        {
            DocID = "";
            Filename = "";
            Path = "";
            MimeType = "";
            Size = 0;
            LastModified = DateTime.Today;
        }
    }
}
