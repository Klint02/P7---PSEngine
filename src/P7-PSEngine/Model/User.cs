using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace P7_PSEngine.Model;

[Index(nameof(Username), IsUnique = true)]
public class User
{
    [Required]
    public int Id { get; set; }

    [Required]
    public string Username { get; set; }

    [Required]
    public string Password { get; set; }

    public ICollection<DocumentInformation> documentInformations { get; set; }
    public ICollection<InvertedIndex> InvertedIndex { get; set; }
}