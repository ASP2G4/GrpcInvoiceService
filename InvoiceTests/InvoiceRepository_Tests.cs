using Data.Contexts;
using Data.Entities;
using Data.Repositories;
using Microsoft.EntityFrameworkCore;

namespace InvoiceTests;

public class InvoiceRepository_Tests
{
    private readonly IInvoiceRepository _invoiceRepository;
    private readonly DataContext _dataContext;

    public InvoiceRepository_Tests()
    {
        var options = new DbContextOptionsBuilder<DataContext>()
            .UseInMemoryDatabase($"{Guid.NewGuid}")
            .Options;

        _dataContext = new DataContext(options);
        _invoiceRepository = new InvoiceRepository(_dataContext);
    }



    [Fact]
    public async Task AddAsync_ShouldAddInvoice()
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


        var invoice = new InvoiceEntity
        {
            StartDate = DateTime.UtcNow,
            EndDate = DateTime.UtcNow.AddDays(30),
            BookingId = 123,
            UserId = "Test_AddAsync_User",
            CompanyId = company.Id,
            StatusId = status.Id
        };


        // Act
        var result = await _invoiceRepository.AddAsync(invoice);


        // Assert
        Assert.NotNull(result);
        Assert.Equal(invoice.Id, result.Id);        
    }


    [Fact]
    public async Task GetInvoiceAsync_ShouldReturnIEnumerableOfInvoices()
    {
        // Arrange
        var company = new CompanyEntity
        {
            Id = "Test_GetAsync_Company",
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

        var invoice = new InvoiceEntity
        {
            StartDate = DateTime.UtcNow,
            EndDate = DateTime.UtcNow.AddDays(30),
            BookingId = 123,
            UserId = "Test_GetAsync_User",
            CompanyId = company.Id,
            StatusId = status.Id
        };

        _dataContext.Invoices.Add(invoice);
        await _dataContext.SaveChangesAsync();

        //Act
        var result = await _invoiceRepository.GetAllAsync();

        // Assert
        Assert.NotNull(result);
        Assert.NotEmpty(result);
        Assert.IsAssignableFrom<IEnumerable<InvoiceEntity>>(result);
    }

    [Fact]
    public async Task UpdateInvoiceAsync_ShouldReturnTrue()
    {
        //Arrange
        var company = new CompanyEntity
        {
            Id = "Test_UpdateAsync_Company",
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

        var invoice = new InvoiceEntity
        {
            StartDate = DateTime.UtcNow,
            EndDate = DateTime.UtcNow.AddDays(30),
            BookingId = 123,
            UserId = "Test_UpdateAsync_User",
            CompanyId = company.Id,
            StatusId = status.Id
        };

        _dataContext.Invoices.Add(invoice);
        await _dataContext.SaveChangesAsync();

        //Act
        invoice.BookingId = 321;
        var result = await _invoiceRepository.UpdateAsync(invoice);

        //Assert
        var updatedInvoice = await _dataContext.Invoices.FindAsync(invoice.Id);

        Assert.True(result);
        Assert.NotNull(updatedInvoice);
        Assert.Equal(invoice.BookingId, updatedInvoice.BookingId);
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

        var invoice = new InvoiceEntity
        {
            StartDate = DateTime.UtcNow,
            EndDate = DateTime.UtcNow.AddDays(30),
            BookingId = 123,
            UserId = "Test_DeleteAsync_User",
            CompanyId = company.Id,
            StatusId = status.Id
        };

        _dataContext.Invoices.Add(invoice);
        await _dataContext.SaveChangesAsync();

        // Act
        var result = await _invoiceRepository.DeleteAsync(invoice);

        // Assert
        Assert.True(result);
        var deletedInvoice = await _dataContext.Invoices.FindAsync(invoice.Id);
        Assert.Null(deletedInvoice);
    }
}
