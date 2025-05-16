namespace Business.Dtos;

public class InvoiceCreateDto
{
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string UserId { get; set; } = string.Empty;
    public string CompanyId { get; set; } = string.Empty;
    public int StatusId { get; set; }
    public int BookingId { get; set; }
}