using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace P7_PSEngine.Model;

public class CloudService {
    [Required]
    public int Id { get; set; }

    [Required]
    public string ServiceType { get; set; }

    [Required, ForeignKey("User")]
    public User UID { get; set; }

    [Required]
    public string UserDefinedServiceName { get; set; }

    [Required]
    public string refresh_token { get; set; }
}