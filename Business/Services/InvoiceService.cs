using Business.Dtos;
using Data.Entities;
using Data.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Business.Services;

public interface IInvoiceService
{
    Task<bool> AddInvoiceAsync(InvoiceCreateDto invoiceDto);
    Task<bool> DeleteInvoiceAsync(InvoiceEntity invoice);
    Task<IEnumerable<InvoiceDto>?> GetAllInvoicesAsync();
    Task<InvoiceEntity?> GetInvoiceByIdAsync(string id);
    Task<bool> UpdateInvoiceAsync(InvoiceEntity invoice);
}

public class InvoiceService : IInvoiceService
{
    private readonly IInvoiceRepository _invoiceRepository;

    public InvoiceService(IInvoiceRepository invoiceRepository)
    {
        _invoiceRepository = invoiceRepository;
    }

    public async Task<bool> AddInvoiceAsync(InvoiceCreateDto invoiceDto)
    {
        if (invoiceDto == null)
        {
            ArgumentNullException.ThrowIfNull(invoiceDto);
        }

        var entity = new InvoiceEntity
        {
            StartDate = invoiceDto.StartDate,
            EndDate = invoiceDto.EndDate,
            UserId = invoiceDto.UserId,
            CompanyId = invoiceDto.CompanyId,
            StatusId = invoiceDto.StatusId,
            InvoiceDetailsId = invoiceDto.InvoiceDetailsId
        };

        return await _invoiceRepository.AddAsync(entity);
    }

    public async Task<bool> UpdateInvoiceAsync(InvoiceEntity invoice)
    {
        if (invoice == null)
        {
            ArgumentNullException.ThrowIfNull(invoice);
        }
        return await _invoiceRepository.UpdateAsync(invoice);
    }

    public async Task<bool> DeleteInvoiceAsync(InvoiceEntity invoice)
    {
        if (invoice == null)
        {
            ArgumentNullException.ThrowIfNull(invoice);
        }
        return await _invoiceRepository.DeleteAsync(invoice);
    }

    public async Task<IEnumerable<InvoiceDto>?> GetAllInvoicesAsync()
    {      
        var entities = await _invoiceRepository.GetAllAsync();
        if (entities == null)
            return null;

        var invoices = entities.Select(entity => new InvoiceDto
        {
            Id = entity.Id.ToString(),
            StartDate = entity.StartDate,
            EndDate = entity.EndDate,
            
            UserName = entity.User.Name,
            UserAddress = entity.User.Address,
            UserEmail = entity.User.Email,
            UserPhone = entity.User.Phone,

            CompanyName = entity.Company.CompanyName,
            CompanyPhone = entity.Company.CompanyPhone,
            CompanyAddress = entity.Company.CompanyAddress,
            CompanyEmail = entity.Company.CompanyEmail,

            StatusName = entity.Status.StatusName,

            TicketCategory = entity.InvoiceDetails.TicketCategory,
            TicketPrice = entity.InvoiceDetails.TicketPrice,
            AmountOfTickets = entity.InvoiceDetails.AmountOfTickets,
        }).ToList();

        return invoices;    
    }

    public async Task<InvoiceEntity?> GetInvoiceByIdAsync(string id)
    {
        if (string.IsNullOrEmpty(id))
        {
            ArgumentNullException.ThrowIfNull(id);
        }
        return await _invoiceRepository.GetAsync(x => x.Id == id);
    }
}


