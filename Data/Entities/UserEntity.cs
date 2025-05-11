using System.ComponentModel.DataAnnotations;

namespace Data.Entities;



/* Byt ut mot User MicroService */
public class UserEntity
{
    [Key]
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string Name { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Address { get; set; } = null!;
    public string Phone { get; set; } = null!;
    public ICollection<InvoiceEntity> Invoices { get; set; } = [];
}

