using Data.Contexts;
using Data.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Data.Repositories;

public interface IInvoiceRepository
{
    Task<InvoiceEntity> AddAsync(InvoiceEntity entity);
    Task<bool> DeleteAsync(InvoiceEntity entity);
    Task<IEnumerable<InvoiceEntity>> GetAllAsync();
    Task<InvoiceEntity?> GetAsync(Expression<Func<InvoiceEntity, bool>> expression);
    Task<bool> UpdateAsync(InvoiceEntity entity);
}

public class InvoiceRepository : IInvoiceRepository
{
    protected readonly DataContext _context;
    protected readonly DbSet<InvoiceEntity> _dbSet;

    public InvoiceRepository(DataContext context)
    {
        _context = context;
        _dbSet = context.Set<InvoiceEntity>();
    }


    /*Create*/
    public async Task<InvoiceEntity> AddAsync(InvoiceEntity entity)
    {  
        ArgumentNullException.ThrowIfNull(entity);     

        await _dbSet.AddAsync(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    /*Read*/
    public async Task<IEnumerable<InvoiceEntity>> GetAllAsync()
    {
        return await _dbSet
            .Include(i => i.Company)
            .Include(i => i.Status)
            .ToListAsync();
    }

    public async Task<InvoiceEntity?> GetAsync(Expression<Func<InvoiceEntity, bool>> expression)
    {
        if (expression == null)
            return null;

        var entity = await _dbSet
            .Include(i => i.Company)
            .Include(i => i.Status)
            .FirstOrDefaultAsync(expression);
        return entity;
    }

    /*Update*/
    public async Task<bool> UpdateAsync(InvoiceEntity entity)
    {
        ArgumentNullException.ThrowIfNull(entity);        

        _dbSet.Update(entity);
        await _context.SaveChangesAsync();
        return true;
    }

    /*Delete*/
    public async Task<bool> DeleteAsync(InvoiceEntity entity)
    {
        ArgumentNullException.ThrowIfNull(entity);           
       
        _dbSet.Remove(entity);
        await _context.SaveChangesAsync();
        return true;
    }
}
