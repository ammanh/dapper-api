using ASPNetCoreDapper.Contracts;
using ASPNetCoreDapper.Context;
using ASPNetCoreDapper.Entities;
using Dapper;

namespace ASPNetCoreDapper.Repository
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly DapperContext _context;

        public CustomerRepository(DapperContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Customer>> GetCustomers()
        {
            var query = "SELECT * FROM Customers";
            using (var connection = _context.CreateConnection())
            {
                var customers = await connection.QueryAsync<Customer>(query);
                return customers.ToList();
            }
        }

        public async Task<Customer> GetCustomer(int id)
        {
            var query = "SELECT * FROM Customers WHERE Id = @Id";
            using (var connection = _context.CreateConnection())
            {
                var customer = await connection.QuerySingleOrDefaultAsync<Customer>(query, new { id });
                return customer;
            }
        }
    }
}