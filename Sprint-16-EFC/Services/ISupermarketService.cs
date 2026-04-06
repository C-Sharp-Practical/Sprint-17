using System.Collections.Generic;
using System.Threading.Tasks;
using EFC.Models;

namespace EFC.Services;

public interface ISupermarketService
{
    Task<IQueryable<Supermarket>> GetAllAsync();
    Task<Supermarket?> GetByIdAsync(int id);
    Task CreateAsync(Supermarket sm);
    Task UpdateAsync(Supermarket sm);
    Task DeleteAsync(int id);
    Task<bool> ExistsAsync(int id);
}
