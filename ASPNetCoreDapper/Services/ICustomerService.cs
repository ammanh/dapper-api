using ASPNetCoreDapper.Dto;
using ASPNetCoreDapper.Entities;

namespace ASPNetCoreDapper.Services
{
    public interface ICustomerService
    {
        Task<IEnumerable<Customer>> GetAllCustomers();
        Task<Customer> GetCustomerById(int id);
        Task<Customer> CreateCustomer(CustomerForCreationDto customerDto);
        Task UpdateCustomer(int id, CustomerForUpdateDto customerDto);
        Task PatchCustomer(int id, CustomerForPatchDto customerDto);
        Task DeleteCustomer(int id);
    }
} 