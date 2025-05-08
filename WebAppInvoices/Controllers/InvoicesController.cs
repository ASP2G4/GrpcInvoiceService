using Business.Dtos;
using Business.Services;
using Data.Entities;
using Data.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace WebAppInvoices.Controllers;

[Route("api/[controller]")]
[ApiController]
public class InvoicesController : ControllerBase
{
    private readonly IInvoiceService _invoiceService;

    public InvoicesController(IInvoiceService invoiceService)
    {
        _invoiceService = invoiceService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var invoices = await _invoiceService.GetAllInvoicesAsync();
        return Ok(invoices);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(string id)
    {
        var project = await _invoiceService.GetInvoiceByIdAsync(id);
        if (project == null)
            return NotFound();

        return Ok(project);
    }

    [HttpPost]
    public async Task<IActionResult> Post(InvoiceCreateDto invoice)
    {
        if (ModelState.IsValid)
        {
            var result = await _invoiceService.AddInvoiceAsync(invoice);
            if (result)
                return Ok(invoice);
            else
                return BadRequest("Failed to create invoice.");
        }

        return BadRequest(ModelState);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Put(InvoiceEntity invoice)
    {
        if (ModelState.IsValid)
        {
            var result = await _invoiceService.UpdateInvoiceAsync(invoice);
            if (result)
                return Ok(invoice);
            else
                return NotFound("Invoice did not update.");
        }
        return BadRequest(ModelState);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        var invoice = await _invoiceService.GetInvoiceByIdAsync(id);
        if (invoice == null)
            return NotFound("No Invoice found with this Id.");

        var result = await _invoiceService.DeleteInvoiceAsync(invoice);
        if (result)
            return Ok("Invoice successfully deleted");
        else
            return BadRequest("Failed to delete invoice.");
    }
}
