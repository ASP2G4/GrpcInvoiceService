// Har använt AI-assistans för texter, dummydata och få upp diagramen i GitHub

# Invoice gRPC Service (.NET 9)

A .NET 9 gRPC service for managing invoices, designed for integration with distributed systems and asynchronous messaging via Azure Service Bus. The service includes full CRUD support, relational data access with EF Core, and real-time invoice creation through background messaging.

---

## ✨ Features

* gRPC API with Proto definitions
* Invoice creation via Azure Service Bus
* Entity Framework Core with SQL Server
* REST API controller for Web interaction
* BackgroundService for message handling
* DTO mapping & clean architecture
* Swagger (OpenAPI) support

---

## 🎓 gRPC Testing Tools (Optional)

* [Postman (with gRPC support)](https://www.postman.com/)

---

## 🚀 Setup

```bash
git clone <your-repo-url>
cd <your-folder-name>
dotnet restore
dotnet run
```

Ensure your `appsettings.json` contains the correct connection strings for:

* SQL Server (`SqlConnection`)
* Azure Service Bus (`ServiceBus` and `InvoiceQueueName`)

---

## 🔗 gRPC Endpoints

All endpoints are defined in the `Protos/invoice.proto` file.

### Overview

This service defines the following gRPC methods:

* `CreateInvoice`
* `GetInvoices`
* `GetInvoiceById`
* `UpdateInvoice`
* `DeleteInvoice`

---

## 🔢 1. CreateInvoice

**Description:** Creates a new invoice for a booking.

**Example Request:**

```json
{
  "start_date": {
    "seconds": 1747008000,
    "nanos": 0
  },
  "end_date": {
    "seconds": 1747612800,
    "nanos": 0
  },
  "user_id": "2ab57586-e49e-465f-ae3f-fd28b828cd25",
  "company_id": "2e5233ed-4136-4067-876d-2ec9bcd3e9e5",
  "status_id": 2,
  "booking_id": 12
}
```

**Example Response:**

```json
{
  "id": 31,
  "start_date": {
    "seconds": 1747008000,
    "nanos": 0
  },
  "end_date": {
    "seconds": 1747612800,
    "nanos": 0
  },
  "user_id": "2ab57586-e49e-465f-ae3f-fd28b828cd25",
  "company_id": "2e5233ed-4136-4067-876d-2ec9bcd3e9e5",
  "status_id": 2,
  "booking_id": 12
}
```

---

## 📁 Project Structure

```
├── Protos/                  # gRPC proto definitions
├── WebAppInvoices/         # REST API with controller for testing
├── Infrastructure/         # Messaging and Azure Service Bus listener
├── Business/               # Business logic and DTO mapping
├── Data/                   # EF Core context, repositories, and entities
```

---

## 🔧 Usage

* File Location: All gRPC contracts are in `Protos/invoice.proto`
* Access via gRPC client (Postman or other tools)

---

## ✅ Testing

* Use Postman or `grpcurl` to interact with gRPC
* Listen for Azure Service Bus messages (handled in `InvoiceServiceBusListener`)

---

## 📄 Notes

* Ensure the database is seeded with at least one `Company` and `Status`
* Company ID is currently hardcoded for demo purposes in `InvoiceServiceBusListener`
* AI-assisted development: text content, sample data, and diagrams generated or assisted via AI

 
