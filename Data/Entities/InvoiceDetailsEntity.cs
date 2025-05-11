using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Data.Entities;

/* Byt ut mot Bookings MicroService */
public class InvoiceDetailsEntity
{
    [Key]
    public string Id { get; set; } = Guid.NewGuid().ToString();

    public string TicketCategory { get; set; } = null!;

    [Required]
    [Precision(18, 2)]
    public decimal TicketPrice { get; set; }

    [Required]
    public int AmountOfTickets { get; set; }
}
