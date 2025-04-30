using ASPNetCoreDapper.Dto;
using ASPNetCoreDapper.Entities;

namespace ASPNetCoreDapper.Contracts
{
    public interface ICustomerRepository
    {
        Task<IEnumerable<Customer>> GetCustomers();
        Task<Customer> GetCustomer(int id);
        Task<Customer> CreateCustomer(CustomerForCreationDto customer);
        Task UpdateCustomer(int id, CustomerForUpdateDto customer);
        Task DeleteCustomer(int id);
    }
}
