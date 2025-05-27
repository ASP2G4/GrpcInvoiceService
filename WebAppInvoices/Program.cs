using Business.Grpc;
using Business.Services;
using Data.Contexts;
using Data.Repositories;
using Infrastructure.Messaging.ServiceBus;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();

builder.Services.AddGrpc();
builder.Services.AddHostedService<InvoiceServiceBusListener>();
builder.Services.AddScoped<IInvoiceRepository, InvoiceRepository>();
builder.Services.AddScoped<IInvoiceService, InvoiceService>();

builder.Services.AddDbContext<DataContext>(x => x.UseSqlServer(builder.Configuration.GetConnectionString("SqlConnection")));

var app = builder.Build();


app.MapGrpcService<InvoiceGrpcService>();
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.MapOpenApi();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();


/*�versikt*/
/*
Detaljerad Vy av vald Invoice.                          -- klart (Du kan h�mta enskilda invoices via Id)
Lista med Invoices.                                     -- klart
CRUD p� Invoice, koppla den till en kund. (Hela CRUD)   -- klart
S�ka i listan av invoices.                              -- klart

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

/*Ticket Details - h�mta sen fr�n egen microservice (Bookings)*/
/*
 * Kategori
 * Pris
 * Qty
 * Pris
 * Total
*/