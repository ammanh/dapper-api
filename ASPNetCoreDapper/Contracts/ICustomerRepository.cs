using ASPNetCoreDapper.Entities;

namespace ASPNetCoreDapper.Contracts
{
    public interface ICustomerRepository
    {
        Task<IEnumerable<Customer>> GetCustomers();
        Task<Customer> GetCustomer(int id);
    }
}
