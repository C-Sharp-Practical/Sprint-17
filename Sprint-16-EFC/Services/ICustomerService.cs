using System.Collections.Generic;
using System.Threading.Tasks;
using EFC.Models;

namespace EFC.Services;

public interface ICustomerService
{
    Task<IEnumerable<Customer>> GetAllAsync(string? name = null, string sortBy = "LastName", bool isAscending = true);
    Task<Customer?> GetByIdAsync(int id);
    Task CreateAsync(Customer customer);
    Task UpdateAsync(Customer customer);
    Task DeleteAsync(int id);
    Task<bool> ExistsAsync(int id);
}
