public class InvoiceDto
{
    public int Id { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }

    public string UserId { get; set; } = null!;
    public string CompanyId { get; set; } = null!;
    public int StatusId { get; set; }
    public int BookingId { get; set; }

    public string? CompanyName { get; set; }
    public string? CompanyEmail { get; set; }
    public string? CompanyAddress { get; set; }
    public string? CompanyPhone { get; set; }

    public string? StatusName { get; set; }
}