namespace Business.Dtos
{
    public class InvoiceDto
    {
        public string Id { get; set; } = null!;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
 
        public string UserName { get; set; } = null!;
        public string UserEmail { get; set; } = null!;
        public string UserAddress { get; set; } = null!;
        public string UserPhone { get; set; } = null!;

        public string CompanyName { get; set; } = null!;
        public string CompanyEmail { get; set; } = null!;
        public string CompanyAddress { get; set; } = null!;
        public string CompanyPhone { get; set; } = null!;

        public string StatusName { get; set; } = null!; 

        public string TicketCategory { get; set; } = null!;
        public decimal TicketPrice { get; set; }
        public int AmountOfTickets { get; set; }
    }
}
