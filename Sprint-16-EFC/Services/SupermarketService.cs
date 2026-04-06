using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using EFC.Data;
using EFC.Models;

namespace EFC.Services;

public class SupermarketService : ISupermarketService
{
    private readonly ShoppingContext _context;

    public SupermarketService(ShoppingContext context)
    {
        _context = context;
    }

    public async Task<IQueryable<Supermarket>> GetAllAsync()
    {
        return _context.Supermarkets.AsQueryable();
    }

    public async Task<Supermarket?> GetByIdAsync(int id)
    {
        return await _context.Supermarkets.FirstOrDefaultAsync(s => s.Id == id);
    }

    public async Task CreateAsync(Supermarket sm)
    {
        _context.Supermarkets.Add(sm);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Supermarket sm)
    {
        _context.Supermarkets.Update(sm);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var entity = await _context.Supermarkets.FindAsync(id);
        if (entity != null)
        {
            _context.Supermarkets.Remove(entity);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<bool> ExistsAsync(int id)
    {
        return await _context.Supermarkets.AnyAsync(s => s.Id == id);
    }
}
