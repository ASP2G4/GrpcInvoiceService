using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Entities;

public class InvoiceEntity
{
    [Key]
    public string Id { get; set; } = Guid.NewGuid().ToString();

    [Required]
    public DateTime StartDate { get; set; } = DateTime.UtcNow;

    [Required]
    public DateTime EndDate { get; set; }


    [ForeignKey(nameof(User))]
    [Required]
    public string UserId { get; set; } = null!;
    public UserEntity User { get; set; } = null!;


    [ForeignKey(nameof(Company))]
    [Required]
    public string CompanyId { get; set; } = null!;
    public CompanyEntity Company { get; set; } = null!;


    [ForeignKey(nameof(Status))]
    [Required]
    public int StatusId { get; set; }
    public StatusEntity Status { get; set; } = null!;


    [ForeignKey(nameof(InvoiceDetails))]
    [Required]
    public string InvoiceDetailsId { get; set; } = null!;
    public InvoiceDetailsEntity InvoiceDetails { get; set; } = null!;
}

