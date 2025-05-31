using Business.Dtos;
using Business.Services;
using Data.Contexts;
using Data.Entities;
using Data.Repositories;
using Microsoft.EntityFrameworkCore;

namespace InvoiceTests;

public class InvoiceService_Tests
{
    private readonly IInvoiceService _invoiceService;
    private readonly IInvoiceRepository _invoiceRepository;
    private readonly DataContext _dataContext;

    public InvoiceService_Tests()
    {
        var options = new DbContextOptionsBuilder<DataContext>()
            .UseInMemoryDatabase($"InvoiceServiceTests-{Guid.NewGuid()}")
            .Options;

        _dataContext = new DataContext(options);
        _invoiceRepository = new InvoiceRepository(_dataContext);
        _invoiceService = new InvoiceService(_invoiceRepository);
    }

    [Fact]
    public async Task AddInvoiceAsync_ShouldAddInvoice()
    {
        // Arrange
        var company = new CompanyEntity
        {
            Id = "Test_AddAsync_Company",
            CompanyName = "Test Company",
            CompanyEmail = "test@company.com",
            CompanyAddress = "123 Test",
            CompanyPhone = "0700-000000"
        };

        var status = new StatusEntity
        {
            StatusName = "Pending"
        };

        _dataContext.Companies.Add(company);
        _dataContext.Statuses.Add(status);
        await _dataContext.SaveChangesAsync();

        var invoiceDto = new InvoiceCreateDto
        {
            StartDate = DateTime.UtcNow,
            EndDate = DateTime.UtcNow.AddDays(30),
            BookingId = 123,
            UserId = "Test_AddAsync_User",
            CompanyId = company.Id,
            StatusId = status.Id
        };

        // Act
        var result = await _invoiceService.AddInvoiceAsync(invoiceDto);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(invoiceDto.UserId, result.UserId);
    }



    [Fact]
    public async Task GetAllInvoicesAsync_ShouldReturnInvoices()
    {
        // Arrange
        var company = new CompanyEntity
        {
            Id = "Test_GetAllAsync_Company",
            CompanyName = "Test Company",
            CompanyEmail = "test@company.com",
            CompanyAddress = "123 Test",
            CompanyPhone = "0700-000000"
        };

        var status = new StatusEntity {
            StatusName = "Pending"
        };

        _dataContext.Companies.Add(company);
        _dataContext.Statuses.Add(status);
        await _dataContext.SaveChangesAsync();

        var invoice = new InvoiceEntity
        {
            StartDate = DateTime.UtcNow,
            EndDate = DateTime.UtcNow.AddDays(30),
            BookingId = 123,
            UserId = "Test_GetAllInvoicesAsync_User",
            CompanyId = company.Id,
            StatusId = status.Id
        };

        _dataContext.Invoices.Add(invoice);
        await _dataContext.SaveChangesAsync();

        // Act
        var result = await _invoiceService.GetAllInvoicesAsync();

        // Assert
        Assert.NotNull(result);
        Assert.NotEmpty(result);
        Assert.Contains(result, i => i.UserId == "Test_GetAllInvoicesAsync_User");
    }

    [Fact]
    public async Task GetInvoiceById_ShouldReturnInvoiceEntity()
    {
        // Arrange
        var company = new CompanyEntity
        {
            Id = "Test_GetInvoiceById_Company",
            CompanyName = "Test Company",
            CompanyEmail = "test@company.com",
            CompanyAddress = "123 Test",
            CompanyPhone = "0700-000000"
        };

        var status = new StatusEntity
        {
            StatusName = "Pending"
        };

        _dataContext.Companies.Add(company);
        _dataContext.Statuses.Add(status);
        await _dataContext.SaveChangesAsync();

        var invoiceDto = new InvoiceCreateDto
        {
            StartDate = DateTime.UtcNow,
            EndDate = DateTime.UtcNow.AddDays(30),
            BookingId = 123,
            UserId = "Test_GetInvoiceByIdAsync_User",
            CompanyId = company.Id,
            StatusId = status.Id
        };

        var addedInvoice = await _invoiceService.AddInvoiceAsync(invoiceDto);

        // Act
        var invoice = await _invoiceService.GetInvoiceByIdAsync(addedInvoice.Id);

        // Assert
        Assert.NotNull(invoice);
        Assert.Equal(addedInvoice.Id, invoice.Id);
    }


    [Fact]
    public async Task UpdateInvoiceAsync_ShouldReturnTrue()
    {
        // Arrange
        var company = new CompanyEntity
        {
            Id = "Test_AddAsync_Company",
            CompanyName = "Test Company",
            CompanyEmail = "test@company.com",
            CompanyAddress = "123 Test",
            CompanyPhone = "0700-000000"
        };

        var status = new StatusEntity
        {
            StatusName = "Pending"
        };

        _dataContext.Companies.Add(company);
        _dataContext.Statuses.Add(status);
        await _dataContext.SaveChangesAsync();

        var invoiceDto = new InvoiceCreateDto
        {
            StartDate = DateTime.UtcNow,
            EndDate = DateTime.UtcNow.AddDays(30),
            BookingId = 123,
            UserId = "Test_UpdateAsync_User",
            CompanyId = company.Id,
            StatusId = status.Id
        };

        var addedInvoice = await _invoiceService.AddInvoiceAsync(invoiceDto);

        // Act
        var updatedInvoice = new InvoiceDto
        {
            Id = addedInvoice.Id,
            StartDate = DateTime.UtcNow.AddDays(1),
            EndDate = DateTime.UtcNow.AddDays(31),
            BookingId = 1234,
            UserId = "Test_UpdateAsync_User",
            CompanyId = company.Id,
            StatusId = status.Id
        };
        var result = await _invoiceService.UpdateInvoiceAsync(updatedInvoice);

        // Assert
        Assert.True(result);
        var invoice = await _invoiceService.GetInvoiceByIdAsync(addedInvoice.Id);
        Assert.NotNull(invoice);
        Assert.Equal(updatedInvoice.StartDate, invoice.StartDate);
        Assert.Equal(updatedInvoice.EndDate, invoice.EndDate);
    }

    [Fact]
    public async Task DeleteInvoiceAsync_ShouldReturnTrue()
    {
        // Arrange
        var company = new CompanyEntity
        {
            Id = "Test_DeleteAsync_Company",
            CompanyName = "Test Company",
            CompanyEmail = "test@company.com",
            CompanyAddress = "123 Test",
            CompanyPhone = "0700-000000"
        };

        var status = new StatusEntity
        {
            StatusName = "Pending"
        };

        _dataContext.Companies.Add(company);
        _dataContext.Statuses.Add(status);
        await _dataContext.SaveChangesAsync();

        var invoiceDto = new InvoiceCreateDto
        {
            StartDate = DateTime.UtcNow,
            EndDate = DateTime.UtcNow.AddDays(30),
            BookingId = 123,
            UserId = "Test_DeleteAsync_User",
            CompanyId = company.Id,
            StatusId = status.Id
        };

        var addedInvoice = await _invoiceService.AddInvoiceAsync(invoiceDto);
        var invoiceEntity = await _dataContext.Invoices.FindAsync(addedInvoice.Id);

        // Act
        var result = await _invoiceService.DeleteInvoiceAsync(invoiceEntity);

        // Assert
        Assert.True(result);
        var deletedInvoice = await _invoiceService.GetInvoiceByIdAsync(addedInvoice.Id);
        Assert.Null(deletedInvoice);
    }
}
