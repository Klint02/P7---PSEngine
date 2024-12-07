using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace P7_PSEngine.Model;

//[Index(nameof(Username), IsUnique = true)]
public class User
{
    
    public int UserId { get; set; }

    
    public string UserName { get; set; }

    
    public string Password { get; set; }

    public ICollection<FileInformation> FileInformations { get; set; }
    public ICollection<InvertedIndex> InvertedIndex { get; set; }
    public ICollection<CloudService> CloudServices { get; set; }
}