using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using EFC.Data;
using EFC.Models;

namespace EFC.Services;

public class CustomerService : ICustomerService
{
    private readonly ShoppingContext _context;

    public CustomerService(ShoppingContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Customer>> GetAllAsync(string? name = null, string sortBy = "LastName", bool isAscending = true)
    {
        IQueryable<Customer> query = _context.Customers.AsQueryable();

        if (!string.IsNullOrEmpty(name))
        {
            query = query.Where(c => c.FirstName.Contains(name) || c.LastName.Contains(name));
        }

        if (sortBy == "Address")
        {
            query = isAscending ? query.OrderBy(c => c.Address) 
                : query.OrderByDescending(c => c.Address);
        }
        else
        {
            query = isAscending ? query.OrderBy(c => c.LastName) 
                : query.OrderByDescending(c => c.LastName);
        }

        return await query.ToListAsync();
    }

    public async Task<Customer?> GetByIdAsync(int id)
    {
        return await _context.Customers.FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task CreateAsync(Customer customer)
    {
        _context.Customers.Add(customer);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Customer customer)
    {
        _context.Customers.Update(customer);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var entity = await _context.Customers.FindAsync(id);
        if (entity != null)
        {
            _context.Customers.Remove(entity);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<bool> ExistsAsync(int id)
    {
        return await _context.Customers.AnyAsync(c => c.Id == id);
    }
}
