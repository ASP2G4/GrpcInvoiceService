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


/*Översikt*/
/*
Lista med Invoices.
Detaljerad Vy av vald Invoice.
CRUD på Invoice, koppla den till en kund. (Hela CRUD)
Söka i listan av invoices.
Sortera invoice listan. (All, Unpaid)
Skicka invoice till kund via email.
Ladda ned invoice.
*/

/*Lista med invoices*/
/*
 * En Invoice MÅSTE ha ett ID
 * En Invoice MÅSTE ha Start datum och Slut datum med tid (HH-MM).
 * En Invoice MÅSTE ha en Status.
 * En Invoice MÅSTE ha en utfärdare (företaget Ventixe)
 * En Invoice MÅSTE ha en mottagare (Användaren på appen - lägga in egen user? eller hämta från annan microservice?)
 * En Invoice ska hämta biljett uppgifter och speca dessa under "Ticket Details" (Göra egen eller hämta från annan microservice?). 
 */

/*Ticket Details - flytta till egen microservice?*/
/*
 * Kategori
 * Pris
 * Qty
 * Pris
 * Total
*/