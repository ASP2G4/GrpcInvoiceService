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
    Task<InvoiceEntity?> GetInvoiceByIdAsync(string id);
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

        var success = await _invoiceRepository.AddAsync(entity);
        return success ? entity : null;
    }


    public async Task<bool> UpdateInvoiceAsync(InvoiceDto invoice)
    {
        if (invoice == null)
        {
            ArgumentNullException.ThrowIfNull(invoice);
        }

        var invoiceEntity = await _invoiceRepository.GetAsync(x => x.Id == invoice.Id);  
            ArgumentNullException.ThrowIfNull(invoiceEntity);

        invoiceEntity.StartDate = invoice.StartDate;
        invoiceEntity.EndDate = invoice.EndDate;

        invoiceEntity.User.Name = invoice.UserName;
        invoiceEntity.User.Email = invoice.UserEmail;
        invoiceEntity.User.Address = invoice.UserAddress;
        invoiceEntity.User.Phone = invoice.UserPhone;

        invoiceEntity.Company.CompanyName = invoice.CompanyName;
        invoiceEntity.Company.CompanyEmail = invoice.CompanyEmail;
        invoiceEntity.Company.CompanyAddress = invoice.CompanyAddress;
        invoiceEntity.Company.CompanyPhone = invoice.CompanyPhone;

        invoiceEntity.InvoiceDetails.TicketCategory = invoice.TicketCategory;
        invoiceEntity.InvoiceDetails.TicketPrice = invoice.TicketPrice;
        invoiceEntity.InvoiceDetails.AmountOfTickets = invoice.AmountOfTickets;

        invoiceEntity.Status.StatusName = invoice.StatusName;

        return await _invoiceRepository.UpdateAsync(invoiceEntity);
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


