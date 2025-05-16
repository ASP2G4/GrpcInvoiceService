using Azure.Messaging.ServiceBus;
using Business.Dtos;
using Business.Services;
using Infrastructure.Messaging.ServiceBus.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace Infrastructure.Messaging.ServiceBus;


public class InvoiceServiceBusListener : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ServiceBusProcessor _processor;
    private readonly ILogger<InvoiceServiceBusListener> _logger;

    public InvoiceServiceBusListener(IConfiguration configuration, ILogger<InvoiceServiceBusListener> logger, IServiceProvider serviceProvider)
    {
        _logger = logger;

        var connectionString = configuration.GetConnectionString("ServiceBus");
        var queueName = configuration["ServiceBus:InvoiceQueueName"];

        var client = new ServiceBusClient(connectionString);
        _processor = client.CreateProcessor(queueName, new ServiceBusProcessorOptions());
        _serviceProvider = serviceProvider;
    }


    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _processor.ProcessMessageAsync += HandleMessageAsync;
        _processor.ProcessErrorAsync += HandleErrorAsync;

        _logger.LogInformation("Starting Service Bus listener...");
        await _processor.StartProcessingAsync(stoppingToken);
    }

    private async Task HandleMessageAsync(ProcessMessageEventArgs args)
    {
        //AI - Måste tydligen använda scoped service provider för att få tillgång till IInvoiceService då InvoiceServiceBusListener är en singleton och den inte kan injicera IInvoiceService direkt.
        using var scope = _serviceProvider.CreateScope();
        var invoiceService = scope.ServiceProvider.GetRequiredService<IInvoiceService>();

        var body = args.Message.Body.ToString();
        _logger.LogInformation("Received message: {Body}", body);

        try
        {
            //så den inte bryr sig om Pascal / Camel case 
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var model = JsonSerializer.Deserialize<BookingCreatedMessage>(
                body, options);

            if (model == null)
            {
                _logger.LogWarning("Received null message");
                await args.AbandonMessageAsync(args.Message);
                return;
            }

            var createInvoice = new InvoiceCreateDto 
            {
                StartDate = DateTime.UtcNow,
                EndDate = DateTime.UtcNow.AddDays(30),
                UserId = model.UserId,
                CompanyId = "2e5233ed-4136-4067-876d-2ec9bcd3e9e5", //hårdkodar så det alltid är "vårt" företag som skickar fakturan.
                StatusId = 1,
                BookingId = model.BookingId
            };

            var result = await invoiceService.AddInvoiceAsync(createInvoice);

            await args.CompleteMessageAsync(args.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to process message");
            await args.AbandonMessageAsync(args.Message);
        }
    }

    private Task HandleErrorAsync(ProcessErrorEventArgs args)
    {
        _logger.LogError(args.Exception, "Service Bus error in {EntityPath}", args.EntityPath);
        return Task.CompletedTask;
    }

    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        await _processor.StopProcessingAsync(cancellationToken);
        await base.StopAsync(cancellationToken);
    }
}
