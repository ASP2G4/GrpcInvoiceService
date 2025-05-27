using Business.Dtos;
using Business.Services;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Protos.GrpcServices;


namespace Business.Grpc;

public class InvoiceGrpcService(IInvoiceService invoiceService) : Protos.GrpcServices.InvoiceService.InvoiceServiceBase
{
    private readonly IInvoiceService _invoiceService = invoiceService;

    public override async Task<CreateInvoiceResponse> CreateInvoice(CreateInvoiceRequest request, ServerCallContext context)
    { 
        try
        {
            var invoiceDto = new InvoiceCreateDto
            {
                StartDate = request.StartDate.ToDateTime().ToUniversalTime(),
                EndDate = request.EndDate.ToDateTime().ToUniversalTime(),
                UserId = request.UserId,
                CompanyId = request.CompanyId,
                StatusId = request.StatusId,
                BookingId = request.BookingId
            };

            var result = await _invoiceService.AddInvoiceAsync(invoiceDto);

            return result == null
                ? throw new RpcException(new Status(StatusCode.Internal, "Failed to create invoice."))
                : new CreateInvoiceResponse
            {
                Id = result.Id,
                StartDate = Timestamp.FromDateTime(result.StartDate.ToUniversalTime()),
                EndDate = Timestamp.FromDateTime(result.EndDate.ToUniversalTime()),
                UserId = result.UserId,
                CompanyId = result.CompanyId,
                StatusId = result.StatusId,
                BookingId = result.BookingId
            };
        }

        catch (Exception ex)
        {
            throw new RpcException(new Status(StatusCode.Unknown, ex.Message));
        }
    }

    public override async Task<GetInvoiceResponse> GetInvoices(Empty request, ServerCallContext context)
    {
        var invoices = await _invoiceService.GetAllInvoicesAsync();

        GetInvoiceResponse response = new();

        response.Invoices.AddRange(invoices?.Select(dto => new Protos.GrpcServices.Invoice
        {
            Id = dto.Id,
            StartDate = Timestamp.FromDateTime(dto.StartDate.ToUniversalTime()),
            EndDate = Timestamp.FromDateTime(dto.EndDate.ToUniversalTime()),

            UserId = dto.UserId,
            CompanyId = dto.CompanyId,
            CompanyName = dto.CompanyName,
            CompanyPhone = dto.CompanyPhone,
            CompanyAddress = dto.CompanyAddress,
            CompanyEmail = dto.CompanyEmail,

            StatusId = dto.StatusId,
            StatusName = dto.StatusName,

            BookingId = dto.BookingId
        }));

        return response;
    }


    public override async Task<GetInvoiceByIdResponse> GetInvoiceById(GetInvoiceByIdRequest request, ServerCallContext context)
    {
        var invoice = await _invoiceService.GetInvoiceByIdAsync(request.Id) 
            ?? throw new RpcException(new Status(StatusCode.NotFound, "Invoice not found"));

        var response = new GetInvoiceByIdResponse
        {
            Invoice = new Protos.GrpcServices.Invoice
            {
                Id = invoice.Id,
                StartDate = Timestamp.FromDateTime(invoice.StartDate.ToUniversalTime()),
                EndDate = Timestamp.FromDateTime(invoice.EndDate.ToUniversalTime()),

                UserId = invoice.UserId,
                CompanyId = invoice.Company.Id,

                CompanyName = invoice.Company?.CompanyName,
                CompanyPhone = invoice.Company?.CompanyPhone,
                CompanyAddress = invoice.Company?.CompanyAddress,
                CompanyEmail = invoice.Company?.CompanyEmail,

                StatusId = invoice.Status.Id,
                StatusName = invoice.Status?.StatusName,

                BookingId = invoice.BookingId
            }
        };

        return response;
    }


    public override async Task<UpdateInvoiceResponse> UpdateInvoice(UpdateInvoiceRequest request, ServerCallContext context)
    {
        try
        {
            if (request.Invoice == null)            
                throw new RpcException(new Status(StatusCode.InvalidArgument, "No invoice in the request."));            

            var invoice = request.Invoice;

            var invoiceExists = await _invoiceService.GetInvoiceByIdAsync(invoice.Id) 
                ?? throw new RpcException(new Status(StatusCode.NotFound, "Invoice not found"));

            var invoiceDto = new InvoiceDto
            {
                Id = invoice.Id,
                StartDate = invoice.StartDate.ToDateTime().ToUniversalTime(),
                EndDate = invoice.EndDate.ToDateTime().ToUniversalTime(),

                UserId = invoice.UserId,
                BookingId = invoice.BookingId,
                CompanyId = invoice.CompanyId,

                StatusId = invoice.StatusId,
                StatusName = invoice.StatusName
            };

            var result = await _invoiceService.UpdateInvoiceAsync(invoiceDto);

            return new UpdateInvoiceResponse
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


    public override async Task<DeleteInvoiceResponse> DeleteInvoice(DeleteInvoiceRequest request, ServerCallContext context)
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



