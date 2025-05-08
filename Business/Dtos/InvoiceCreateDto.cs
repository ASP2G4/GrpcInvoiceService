namespace Business.Dtos;

public class InvoiceCreateDto
{
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string UserId { get; set; } = null!;
    public string CompanyId { get; set; } = null!;
    public int StatusId { get; set; }
    public string InvoiceDetailsId { get; set; } = null!;
}