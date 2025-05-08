using System.ComponentModel.DataAnnotations;

namespace Data.Entities;

public class CompanyEntity
{
    [Key]
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string CompanyName { get; set; } = null!;
    public string CompanyEmail { get; set; } = null!;
    public string CompanyAddress { get; set; } = null!;
    public string CompanyPhone { get; set; } = null!;
    public ICollection<InvoiceEntity> Invoices { get; set; } = [];
}