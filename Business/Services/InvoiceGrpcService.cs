using Business.Dtos;
using Business.Services;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Protos.GrpcServices;

namespace Business.Grpc;

public class InvoiceGrpcService(IInvoiceService invoiceService) : Protos.GrpcServices.InvoiceService.InvoiceServiceBase
{
    private readonly IInvoiceService _invoiceService = invoiceService;

    public override async Task<Protos.GrpcServices.CreateInvoiceResponse> CreateInvoice(Protos.GrpcServices.CreateInvoiceRequest request, ServerCallContext context)
    { 
        try
        {
            var invoiceDto = new InvoiceCreateDto
            {
                StartDate = request.StartDate.ToDateTime(),
                EndDate = request.EndDate.ToDateTime(),
                UserId = request.UserId,
                CompanyId = request.CompanyId,
                StatusId = request.StatusId,
                InvoiceDetailsId = request.InvoiceDetailsId
            };

            var result = await _invoiceService.AddInvoiceAsync(invoiceDto);

            return new Protos.GrpcServices.CreateInvoiceResponse
            {
                Id = result.Id,
                StartDate = Google.Protobuf.WellKnownTypes.Timestamp.FromDateTime(result.StartDate),
                EndDate = Google.Protobuf.WellKnownTypes.Timestamp.FromDateTime(result.EndDate),
                UserId = result.UserId,
                CompanyId = result.CompanyId,
                StatusId = result.StatusId,
                InvoiceDetailsId = result.InvoiceDetailsId
            };
        }
        catch (Exception ex)
        {
            throw new RpcException(new Status(StatusCode.Unknown, ex.Message));
        }
    }

    public override async Task<Protos.GrpcServices.GetInvoiceResponse> GetInvoices(Empty request, ServerCallContext context)
    {
        var invoices = await _invoiceService.GetAllInvoicesAsync();

        Protos.GrpcServices.GetInvoiceResponse response = new();

        response.Invoices.AddRange(invoices.Select(dto => new Protos.GrpcServices.Invoice
        {
            Id = dto.Id,
            StartDate = Timestamp.FromDateTime(dto.StartDate.ToUniversalTime()),
            EndDate = Timestamp.FromDateTime(dto.EndDate.ToUniversalTime()),

            UserName = dto.UserName,
            UserAddress = dto.UserAddress,
            UserEmail = dto.UserEmail,
            UserPhone = dto.UserPhone,

            CompanyName = dto.CompanyName,
            CompanyPhone = dto.CompanyPhone,
            CompanyAddress = dto.CompanyAddress,
            CompanyEmail = dto.CompanyEmail,

            StatusName = dto.StatusName,

            TicketCategory = dto.TicketCategory,
            TicketPrice = dto.TicketPrice.ToString("F2"), //Kom ihåg att konvertera till decimal i frontend.
            AmountOfTickets = dto.AmountOfTickets,
        }));

        return response;
    }

    public override async Task<Protos.GrpcServices.GetInvoiceByIdResponse> GetInvoiceById(Protos.GrpcServices.GetInvoiceByIdRequest request, ServerCallContext context)
    {
        var invoice = await _invoiceService.GetInvoiceByIdAsync(request.Id);

        if (invoice == null)
        {
            throw new RpcException(new Status(StatusCode.NotFound, "Invoice not found"));
        }

        var response = new Protos.GrpcServices.GetInvoiceByIdResponse
        {
            Invoice = new Protos.GrpcServices.Invoice
            {
                Id = invoice.Id,
                StartDate = Timestamp.FromDateTime(invoice.StartDate.ToUniversalTime()),
                EndDate = Timestamp.FromDateTime(invoice.EndDate.ToUniversalTime()),

                UserName = invoice.User.Name,
                UserAddress = invoice.User.Address,
                UserEmail = invoice.User.Email,
                UserPhone = invoice.User.Phone,

                CompanyName = invoice.Company.CompanyName,
                CompanyPhone = invoice.Company.CompanyPhone,
                CompanyAddress = invoice.Company.CompanyAddress,
                CompanyEmail = invoice.Company.CompanyEmail,

                StatusName = invoice.Status.StatusName,

                TicketCategory = invoice.InvoiceDetails.TicketCategory,
                TicketPrice = invoice.InvoiceDetails.TicketPrice.ToString("F2"), //Kom ihåg att konvertera till decimal i frontend.
                AmountOfTickets = invoice.InvoiceDetails.AmountOfTickets
            }
        };

        return response;
    }

    public override async Task<Protos.GrpcServices.UpdateInvoiceResponse> UpdateInvoice(Protos.GrpcServices.UpdateInvoiceRequest request, ServerCallContext context)
    {
        try
        {
            if (request.Invoice == null)
            {
                throw new RpcException(new Status(StatusCode.InvalidArgument, "No invoice in the request."));
            }
            var invoice = request.Invoice;

            var invoiceExists = await _invoiceService.GetInvoiceByIdAsync(invoice.Id);
            if (invoiceExists == null)
            {
                throw new RpcException(new Status(StatusCode.NotFound, "Invoice not found"));
            }

            //en-us använder "," medans svensk culture är "." - TryParse för att se så allt blir rätt och man inte skriver in "ABC" istället.
            if (!decimal.TryParse(invoice.TicketPrice, System.Globalization.NumberStyles.Number, System.Globalization.CultureInfo.InvariantCulture, out var ticketPrice)) 
            {
                throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid ticket price format"));
            }

            var invoiceDto = new InvoiceDto
            {
                Id = invoice.Id,
                StartDate = invoice.StartDate.ToDateTime(),
                EndDate = invoice.EndDate.ToDateTime(),

                UserName = invoice.UserName,
                UserAddress = invoice.UserAddress,
                UserEmail = invoice.UserEmail,
                UserPhone = invoice.UserPhone,

                CompanyName = invoice.CompanyName,
                CompanyPhone = invoice.CompanyPhone,
                CompanyAddress = invoice.CompanyAddress,
                CompanyEmail = invoice.CompanyEmail,

                StatusName = invoice.StatusName,

                TicketCategory = invoice.TicketCategory,
                TicketPrice = ticketPrice,
                AmountOfTickets = invoice.AmountOfTickets
            };

            var result = await _invoiceService.UpdateInvoiceAsync(invoiceDto);

            return new Protos.GrpcServices.UpdateInvoiceResponse
            {
                Success = result
            };

        }
        catch (RpcException)
        {
            throw;
        }
        catch (Exception ex)
        {
            throw new RpcException(new Status(StatusCode.Internal, $"Internal server error: {ex.Message}"));
        }
    }

    public override async Task<Protos.GrpcServices.DeleteInvoiceResponse> DeleteInvoice(Protos.GrpcServices.DeleteInvoiceRequest request, ServerCallContext context)
    {
        try
        {
            var invoice = await _invoiceService.GetInvoiceByIdAsync(request.Id)
                          ?? throw new RpcException(new Status(StatusCode.NotFound, "Invoice not found"));

            var result = await _invoiceService.DeleteInvoiceAsync(invoice);

            return new Protos.GrpcServices.DeleteInvoiceResponse
            {
                Success = result
            };
        }
        catch (RpcException)
        {
            throw;
        }
        catch (Exception ex)
        {
            throw new RpcException(new Status(StatusCode.Internal, $"Internal server error: {ex.Message}"));
        }
    }
}



