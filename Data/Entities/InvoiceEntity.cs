using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Entities;

public class InvoiceEntity
{
    [Key]
    public int Id { get; set; }

    [Required]
    public DateTime StartDate { get; set; } = DateTime.UtcNow;

    [Required]
    public DateTime EndDate { get; set; }

    [Required]
    public int BookingId { get; set; }

    [Required]
    public string UserId { get; set; } = null!;


    [ForeignKey(nameof(Company))]
    [Required]
    public string CompanyId { get; set; } = null!;
    public CompanyEntity Company { get; set; } = null!;


    [ForeignKey(nameof(Status))]
    [Required]
    public int StatusId { get; set; }
    public StatusEntity Status { get; set; } = null!;
}

