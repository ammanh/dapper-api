using ASPNetCoreDapper.Contracts;
using ASPNetCoreDapper.Dto;
using ASPNetCoreDapper.Entities;

namespace ASPNetCoreDapper.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _customerRepository;

        public CustomerService(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public async Task<IEnumerable<Customer>> GetAllCustomers()
        {
            return await _customerRepository.GetCustomers();
        }

        public async Task<Customer> GetCustomerById(int id)
        {
            var customer = await _customerRepository.GetCustomer(id);
            if (customer == null)
                throw new KeyNotFoundException($"Customer with ID {id} not found");
            return customer;
        }

        public async Task<Customer> CreateCustomer(CustomerForCreationDto customerDto)
        {
            return await _customerRepository.CreateCustomer(customerDto);
        }

        public async Task UpdateCustomer(int id, CustomerForUpdateDto customerDto)
        {
            var customer = await GetCustomerById(id);
            await _customerRepository.UpdateCustomer(id, customerDto);
        }

        public async Task PatchCustomer(int id, CustomerForPatchDto customerDto)
        {
            var customer = await GetCustomerById(id);

            var updateDto = new CustomerForUpdateDto
            {
                Name = customerDto.Name ?? customer.Name,
                Email = customerDto.Email ?? customer.Email,
                Phone = customerDto.Phone ?? customer.Phone
            };

            await UpdateCustomer(id, updateDto);
        }

        public async Task DeleteCustomer(int id)
        {
            var customer = await GetCustomerById(id);
            await _customerRepository.DeleteCustomer(id);
        }
    }
} 