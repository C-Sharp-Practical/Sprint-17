using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using EFC.Data;
using EFC.Models;

namespace EFC.Services;

public class OrderService : IOrderService
{
    private readonly ShoppingContext _context;

    public OrderService(ShoppingContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Order>> GetAllAsync()
    {
        return await _context.Orders
            .Include(o => o.Customer)
            .Include(o => o.SuperMarket)
            .OrderByDescending(o => o.OrderDate)
            .ToListAsync();
    }

    public async Task<Order?> GetByIdAsync(int id)
    {
        return await _context.Orders
            .Include(o => o.OrderDetails)
                .ThenInclude(d => d.Product)
            .FirstOrDefaultAsync(o => o.Id == id);
    }

    public async Task CreateAsync(Order order)
    {
        _context.Orders.Add(order);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Order order)
    {
        _context.Orders.Update(order);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var entity = await _context.Orders.FindAsync(id);
        if (entity != null)
        {
            _context.Orders.Remove(entity);
            await _context.SaveChangesAsync();
        }
    }

    public async Task AddDetailAsync(OrderDetail detail)
    {
        _context.OrderDetails.Add(detail);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteDetailAsync(int id)
    {
        var detail = await _context.OrderDetails.FindAsync(id);
        if (detail != null)
        {
            _context.OrderDetails.Remove(detail);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<OrderDetail?> GetDetailByIdAsync(int id)
    {
        return await _context.OrderDetails
            .FirstOrDefaultAsync(d => d.Id == id);
    }
}
