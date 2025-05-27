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


/*Översikt*/
/*
Detaljerad Vy av vald Invoice.                          -- klart (Du kan hämta enskilda invoices via Id)
Lista med Invoices.                                     -- klart
CRUD på Invoice, koppla den till en kund. (Hela CRUD)   -- klart
Söka i listan av invoices.                              -- klart

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

/*Ticket Details - hämta sen från egen microservice (Bookings)*/
/*
 * Kategori
 * Pris
 * Qty
 * Pris
 * Total
*/