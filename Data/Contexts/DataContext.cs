using Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Data.Contexts;

public class DataContext(DbContextOptions<DataContext> options) : DbContext(options)
{
    public DbSet<CompanyEntity> Companies { get; set; }
    public DbSet<InvoiceEntity> Invoices { get; set; }
    public DbSet<StatusEntity> Statuses { get; set; }
    public DbSet<UserEntity> Users { get; set; }
}
