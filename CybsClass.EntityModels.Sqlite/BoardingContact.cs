using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CybsClass.EntityModels;

[Table("BoardingContact")]
public class BoardingContact
{
    [Key]
    public int BoardingContactId { get; set; }

    public int BoardingOrganizationId { get; set; }

    [Required]
    [StringLength(50)]
    public string ContactType { get; set; } = null!;   // "Business" | "Technical" | "Emergency"

    [StringLength(100)]
    public string? FirstName { get; set; }

    [StringLength(100)]
    public string? LastName { get; set; }

    [StringLength(30)]
    public string? PhoneNumber { get; set; }

    [StringLength(200)]
    public string? Email { get; set; }

    [ForeignKey("BoardingOrganizationId")]
    public virtual BoardingOrganization BoardingOrganization { get; set; } = null!;
}
