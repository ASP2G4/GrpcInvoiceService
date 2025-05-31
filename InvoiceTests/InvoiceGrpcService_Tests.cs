using Business.Dtos;
using Business.Grpc;
using Business.Services;
using Data.Entities;
using Google.Protobuf.WellKnownTypes;
using Moq;
using Protos.GrpcServices;

namespace InvoiceTests;

public class InvoiceGrpcService_Tests
{
    [Fact]
    public async Task CreateInvoice_ReturnsCreatedInvoiceAsync()
    {
        // Arrange
        var mockService = new Mock<IInvoiceService>();

        var company = new CompanyEntity
        {
            Id = "Test_AddAsync_CompanyId",
            CompanyName = "Test Company",
            CompanyEmail = "test@company.com",
            CompanyAddress = "Test address",
            CompanyPhone = "0700-000000"
        };

        var status = new StatusEntity
        {
            StatusName = "Pending"
        };

        var invoice = new InvoiceEntity
        {
            Id = 1,
            StartDate = DateTime.UtcNow,
            EndDate = DateTime.UtcNow.AddDays(30),
            BookingId = 123,
            UserId = "Test_AddAsync_UserId",
            CompanyId = company.Id,
            StatusId = status.Id
        };

        mockService.Setup(s => s.AddInvoiceAsync(It.IsAny<InvoiceCreateDto>()))
                   .ReturnsAsync(invoice);

        var grpcService = new InvoiceGrpcService(mockService.Object);

        var request = new CreateInvoiceRequest
        {
            StartDate = Timestamp.FromDateTime(DateTime.UtcNow),
            EndDate = Timestamp.FromDateTime(DateTime.UtcNow.AddDays(1)),
            UserId = "Test_AddAsync_UserId",
            CompanyId = "Test_AddAsync_CompanyId",
            StatusId = 1,
            BookingId = 100
        };

        // Act
        var response = await grpcService.CreateInvoice(request, null);

        // Assert
        Assert.Equal(invoice.Id, response.Id);
        Assert.Equal(invoice.UserId, response.UserId);
    }

    [Fact]
    public async Task GetInvoices_ReturnsAllInvoicesAsync()
    {
        // Arrange
        var mockService = new Mock<IInvoiceService>();

        var invoices = new List<InvoiceDto>
        {
            new InvoiceDto
            {
                Id = 1,
                StartDate = DateTime.UtcNow,
                EndDate = DateTime.UtcNow.AddDays(30),
                UserId = "GetInvoices_UserId",
                CompanyId = "GetInvoices_CompanyId",
                CompanyName = "Company Name",
                CompanyPhone = "0700-000000",
                CompanyAddress = "Test Address",
                CompanyEmail = "test@company.com",
                StatusId = 2,
                StatusName = "Pending",
                BookingId = 100
            }
        };

        mockService.Setup(s => s.GetAllInvoicesAsync())
                   .ReturnsAsync(invoices);

        var grpcService = new InvoiceGrpcService(mockService.Object);

        // Act
        var response = await grpcService.GetInvoices(new Empty(), null);

        // Assert
        Assert.Single(response.Invoices);
    }

    [Fact]
    public async Task GetInvoiceById_ReturnsInvoiceAsync()
    {
        // Arrange
        var mockService = new Mock<IInvoiceService>();

        var invoiceEntity = new InvoiceEntity
        {
            Id = 1,
            StartDate = DateTime.UtcNow,
            EndDate = DateTime.UtcNow.AddDays(30),
            UserId = "GetInvoiceById_UserId",
            CompanyId = "GetInvoiceById_CompanyId",
            BookingId = 100,
            StatusId = 2,
            Company = new CompanyEntity
            {
                Id = "GetInvoiceById_CompanyId",
                CompanyName = "Company Name",
                CompanyPhone = "0700-000000",
                CompanyAddress = "Test Address",
                CompanyEmail = "test@company.com"
            },
            Status = new StatusEntity
            {
                Id = 2,
                StatusName = "Pending"
            }
        };

        mockService.Setup(s => s.GetInvoiceByIdAsync(invoiceEntity.Id))
                   .ReturnsAsync(invoiceEntity);

        var grpcService = new InvoiceGrpcService(mockService.Object);

        var request = new GetInvoiceByIdRequest { Id = invoiceEntity.Id };

        // Act
        var response = await grpcService.GetInvoiceById(request, null);

        // Assert
        Assert.NotNull(response.Invoice);
        Assert.Equal(invoiceEntity.Id, response.Invoice.Id);
        Assert.Equal(invoiceEntity.UserId, response.Invoice.UserId);
        Assert.Equal(invoiceEntity.Company.CompanyName, response.Invoice.CompanyName);
        Assert.Equal(invoiceEntity.Status.StatusName, response.Invoice.StatusName);
    }

    [Fact]
    public async Task UpdateInvoice_ReturnsSuccessAsync()
    {
        // Arrange
        var mockService = new Mock<IInvoiceService>();

        //fixedStart och fixedEnd var det AI som tyckte jag kunde förbättra med min kod.
        var fixedStart = new DateTime(2025, 6, 1, 0, 0, 0, DateTimeKind.Utc);
        var fixedEnd = fixedStart.AddDays(30);

        var invoiceEntity = new InvoiceEntity
        {
            Id = 1,
            StartDate = DateTime.UtcNow,
            EndDate = DateTime.UtcNow.AddDays(30),
            UserId = "Test_Update_UserId",
            CompanyId = "Update_CompanyId",
            BookingId = 100,
            StatusId = 2,
            Company = new CompanyEntity
            {
                Id = "Update_CompanyId",
                CompanyName = "Company Name",
                CompanyPhone = "0700-000000",
                CompanyAddress = "Test Address",
                CompanyEmail = "test@company.com"
            },
            Status = new StatusEntity
            {
                Id = 2,
                StatusName = "Pending"
            }
        };

        mockService.Setup(s => s.GetInvoiceByIdAsync(invoiceEntity.Id))
                   .ReturnsAsync(invoiceEntity);

        mockService.Setup(s => s.UpdateInvoiceAsync(It.IsAny<InvoiceDto>()))
                   .ReturnsAsync(true);

        var grpcService = new InvoiceGrpcService(mockService.Object);

        var request = new UpdateInvoiceRequest
        {
            Invoice = new Protos.GrpcServices.Invoice
            {
                Id = 1,
                StartDate = Timestamp.FromDateTime(fixedStart),
                EndDate = Timestamp.FromDateTime(fixedEnd),
                UserId = "Update_UserId",
                BookingId = 100,
                CompanyId = "Update_CompanyId",
                StatusId = 2,
                StatusName = "Pending"
            }
        };

        // Act
        var response = await grpcService.UpdateInvoice(request, null);

        // Assert
        Assert.True(response.Success);
    }

    [Fact]
    public async Task DeleteInvoice_ReturnsSuccessAsync()
    {
        // Arrange
        var mockService = new Mock<IInvoiceService>();

        //fixedStart och fixedEnd var det AI som tyckte jag kunde förbättra med min kod.
        var fixedStart = new DateTime(2025, 6, 1, 0, 0, 0, DateTimeKind.Utc);
        var fixedEnd = fixedStart.AddDays(30);

        var invoiceEntity = new InvoiceEntity
        {
            Id = 1,
            StartDate = DateTime.UtcNow,
            EndDate = DateTime.UtcNow.AddDays(30),
            UserId = "Delete_UserId",
            CompanyId = "Delete_CompanyId",
            BookingId = 100,
            StatusId = 2,
            Company = new CompanyEntity
            {
                Id = "Delete_CompanyId",
                CompanyName = "Company Name",
                CompanyPhone = "0700-000000",
                CompanyAddress = "Test Address",
                CompanyEmail = "test@company.com"
            },
            Status = new StatusEntity
            {
                Id = 2,
                StatusName = "Pending"
            }
        };

        mockService.Setup(s => s.GetInvoiceByIdAsync(invoiceEntity.Id))
                   .ReturnsAsync(invoiceEntity);

        mockService.Setup(s => s.DeleteInvoiceAsync(invoiceEntity))
                   .ReturnsAsync(true);

        var grpcService = new InvoiceGrpcService(mockService.Object);

        var request = new DeleteInvoiceRequest { Id = 1 };

        // Act
        var response = await grpcService.DeleteInvoice(request, null);

        // Assert
        Assert.True(response.Success);
    }
}