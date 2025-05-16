using Azure.Messaging.ServiceBus;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Grpc.Net.Client;
using Protos.GrpcServices;

namespace Invoices.Functions;

public class ServiceBusFunction
{
    private static readonly string grpcServiceAddress = "https://localhost:7153";

    [FunctionName("ServiceBusListener")]
    public static async Task Run(
        [ServiceBusTrigger("booking-created-to-invoice-service", 
        Connection = "ServiceBusConnectionString")] ServiceBusReceivedMessage message,
        ILogger log)
    {
        log.LogInformation($"Received message: {message.Body}");

        var messageBody = message.Body.ToString();

        await ProcessMessageAsync(messageBody);

        log.LogInformation("Message processed successfully.");
    }
        private static Task ProcessMessageAsync(string messageBody)
    {

        return Task.CompletedTask;
    }
}
