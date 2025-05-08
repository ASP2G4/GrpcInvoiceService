using Business.Services;
using Data.Contexts;
using Data.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();

builder.Services.AddScoped<IInvoiceRepository, InvoiceRepository>();

builder.Services.AddScoped<IInvoiceService, InvoiceService>();

builder.Services.AddDbContext<DataContext>(x => x.UseSqlServer(builder.Configuration.GetConnectionString("SqlConnection")));

var app = builder.Build();

app.MapOpenApi();
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();


/*�versikt*/
/*
Lista med Invoices.
Detaljerad Vy av vald Invoice.
CRUD p� Invoice, koppla den till en kund. (Hela CRUD)
S�ka i listan av invoices.
Sortera invoice listan. (All, Unpaid)
Skicka invoice till kund via email.
Ladda ned invoice.
*/

/*Lista med invoices*/
/*
 * En Invoice M�STE ha ett ID
 * En Invoice M�STE ha Start datum och Slut datum med tid (HH-MM).
 * En Invoice M�STE ha en Status.
 * En Invoice M�STE ha en utf�rdare (f�retaget Ventixe)
 * En Invoice M�STE ha en mottagare (Anv�ndaren p� appen - l�gga in egen user? eller h�mta fr�n annan microservice?)
 * En Invoice ska h�mta biljett uppgifter och speca dessa under "Ticket Details" (G�ra egen eller h�mta fr�n annan microservice?). 
 */

/*Ticket Details - flytta till egen microservice?*/
/*
 * Kategori
 * Pris
 * Qty
 * Pris
 * Total
*/