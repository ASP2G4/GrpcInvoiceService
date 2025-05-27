using Business.Dtos;
using Data.Entities;
using Data.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Business.Services;

public interface IInvoiceService
{
    Task<InvoiceEntity?> AddInvoiceAsync(InvoiceCreateDto invoiceDto);
    Task<bool> DeleteInvoiceAsync(InvoiceEntity invoice);
    Task<IEnumerable<InvoiceDto>?> GetAllInvoicesAsync();
    Task<InvoiceEntity?> GetInvoiceByIdAsync(int id);
    Task<bool> UpdateInvoiceAsync(InvoiceDto invoice);
}

public class InvoiceService : IInvoiceService
{
    private readonly IInvoiceRepository _invoiceRepository;

    public InvoiceService(IInvoiceRepository invoiceRepository)
    {
        _invoiceRepository = invoiceRepository;
    }

    public async Task<InvoiceEntity?> AddInvoiceAsync(InvoiceCreateDto invoiceDto)
    {
        if (invoiceDto == null)        
            ArgumentNullException.ThrowIfNull(invoiceDto);
        

        var entity = new InvoiceEntity
        {
            StartDate = invoiceDto.StartDate,
            EndDate = invoiceDto.EndDate,
            UserId = invoiceDto.UserId,
            CompanyId = invoiceDto.CompanyId,
            StatusId = invoiceDto.StatusId,
            BookingId = invoiceDto.BookingId
        };

        var result = await _invoiceRepository.AddAsync(entity);
        return result;
    }

    public async Task<IEnumerable<InvoiceDto>?> GetAllInvoicesAsync()
    {
        var entities = await _invoiceRepository.GetAllAsync();
        if (entities == null)
            return null;

        var invoices = entities.Select(entity => new InvoiceDto
        {
            Id = entity.Id,
            StartDate = entity.StartDate,
            EndDate = entity.EndDate,
            UserId = entity.UserId,

            CompanyId = entity.CompanyId,
            CompanyName = entity.Company.CompanyName,
            CompanyPhone = entity.Company.CompanyPhone,
            CompanyAddress = entity.Company.CompanyAddress,
            CompanyEmail = entity.Company.CompanyEmail,

            StatusId = entity.StatusId,
            StatusName = entity.Status.StatusName,

            BookingId = entity.BookingId
        }).ToList();

        return invoices;
    }

    public async Task<InvoiceEntity?> GetInvoiceByIdAsync(int id)
    {
        if (id <= 0)        
            throw new ArgumentException("Invalid invoice ID.", nameof(id));
        

        return await _invoiceRepository.GetAsync(x => x.Id == id);
    }

    public async Task<bool> UpdateInvoiceAsync(InvoiceDto invoice)
    {
        if (invoice == null)        
            ArgumentNullException.ThrowIfNull(invoice);
        

        var invoiceEntity = await _invoiceRepository.GetAsync(x => x.Id == invoice.Id);
            ArgumentNullException.ThrowIfNull(invoiceEntity);

        invoiceEntity.StartDate = invoice.StartDate;
        invoiceEntity.EndDate = invoice.EndDate;
        invoiceEntity.UserId = invoice.UserId;
        invoiceEntity.CompanyId = invoice.CompanyId;
        invoiceEntity.StatusId = invoice.StatusId;
        invoiceEntity.BookingId = invoice.BookingId;

        return await _invoiceRepository.UpdateAsync(invoiceEntity);
    }

    public async Task<bool> DeleteInvoiceAsync(InvoiceEntity invoice)
    {
        if (invoice == null)        
            ArgumentNullException.ThrowIfNull(invoice);
        
        return await _invoiceRepository.DeleteAsync(invoice);
    }
}


