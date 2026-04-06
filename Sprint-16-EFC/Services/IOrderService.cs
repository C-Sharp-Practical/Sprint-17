using System.Collections.Generic;
using System.Threading.Tasks;
using EFC.Models;

namespace EFC.Services;

public interface IOrderService
{
    Task<IEnumerable<Order>> GetAllAsync();
    Task<Order?> GetByIdAsync(int id);
    Task CreateAsync(Order order);
    Task UpdateAsync(Order order);
    Task DeleteAsync(int id);

    Task AddDetailAsync(OrderDetail detail);
    Task DeleteDetailAsync(int id);
    Task<OrderDetail?> GetDetailByIdAsync(int id);
}
