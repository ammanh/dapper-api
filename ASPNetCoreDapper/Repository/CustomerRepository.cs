using System.Data;
using Dapper;
using ASPNetCoreDapper.Contracts;
using ASPNetCoreDapper.Context;
using ASPNetCoreDapper.Entities;
using ASPNetCoreDapper.Dto;

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
            const string sql = "SELECT * FROM Customers";
            using (var connection = _context.CreateConnection())
            {
                return await connection.QueryAsync<Customer>(sql);
            }
        }

        public async Task<Customer> GetCustomer(int id)
        {
            const string sql = "SELECT * FROM Customers WHERE Id = @Id";
            using (var connection = _context.CreateConnection())
            {
                return await connection.QuerySingleOrDefaultAsync<Customer>(sql, new { Id = id });
            }
        }

        public async Task<Customer> CreateCustomer(CustomerForCreationDto customerDto)
        {
            const string sql = @"
                INSERT INTO Customers (Name, Email, Phone)
                VALUES (@Name, @Email, @Phone);
                SELECT CAST(SCOPE_IDENTITY() as int)";

            using (var connection = _context.CreateConnection())
            {
                var id = await connection.QuerySingleAsync<int>(sql, customerDto);
                
                return new Customer
                {
                    Id = id,
                    Name = customerDto.Name,
                    Email = customerDto.Email,
                    Phone = customerDto.Phone
                };
            }
        }

        public async Task UpdateCustomer(int id, CustomerForUpdateDto customerDto)
        {
            const string sql = @"
                UPDATE Customers
                SET Name = @Name,
                    Email = @Email,
                    Phone = @Phone
                WHERE Id = @Id";

            using (var connection = _context.CreateConnection())
            {
                await connection.ExecuteAsync(sql, new
                {
                    Id = id,
                    customerDto.Name,
                    customerDto.Email,
                    customerDto.Phone
                });
            }
        }

        public async Task DeleteCustomer(int id)
        {
            const string sql = "DELETE FROM Customers WHERE Id = @Id";
            using (var connection = _context.CreateConnection())
            {
                await connection.ExecuteAsync(sql, new { Id = id });
            }
        }
    }
}