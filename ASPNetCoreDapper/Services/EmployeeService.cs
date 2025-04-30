using ASPNetCoreDapper.Contracts;
using ASPNetCoreDapper.Dto;
using ASPNetCoreDapper.Entities;

namespace ASPNetCoreDapper.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly ICompanyRepository _companyRepository;

        public EmployeeService(IEmployeeRepository employeeRepository, ICompanyRepository companyRepository)
        {
            _employeeRepository = employeeRepository;
            _companyRepository = companyRepository;
        }

        public async Task<IEnumerable<Employee>> GetAllEmployees()
        {
            return await _employeeRepository.GetEmployees();
        }

        public async Task<Employee> GetEmployeeById(int id)
        {
            var employee = await _employeeRepository.GetEmployee(id);
            if (employee == null)
                throw new KeyNotFoundException($"Employee with ID {id} not found");
            return employee;
        }

        public async Task<Employee> CreateEmployee(EmployeeForCreationDto employeeDto)
        {
            if (string.IsNullOrWhiteSpace(employeeDto.Name))
                throw new ArgumentException("Employee name is required");

            if (employeeDto.Age <= 0)
                throw new ArgumentException("Employee age must be greater than 0");

            if (string.IsNullOrWhiteSpace(employeeDto.Position))
                throw new ArgumentException("Employee position is required");

            // Verify company exists
            var company = await _companyRepository.GetCompany(employeeDto.CompanyId);
            if (company == null)
                throw new ArgumentException($"Company with ID {employeeDto.CompanyId} not found");

            return await _employeeRepository.CreateEmployee(employeeDto);
        }

        public async Task UpdateEmployee(int id, EmployeeForUpdateDto employeeDto)
        {
            if (string.IsNullOrWhiteSpace(employeeDto.Name))
                throw new ArgumentException("Employee name is required");

            if (employeeDto.Age <= 0)
                throw new ArgumentException("Employee age must be greater than 0");

            if (string.IsNullOrWhiteSpace(employeeDto.Position))
                throw new ArgumentException("Employee position is required");

            // Verify company exists
            var company = await _companyRepository.GetCompany(employeeDto.CompanyId);
            if (company == null)
                throw new ArgumentException($"Company with ID {employeeDto.CompanyId} not found");

            var employee = await GetEmployeeById(id);
            await _employeeRepository.UpdateEmployee(id, employeeDto);
        }

        public async Task PatchEmployee(int id, EmployeeForPatchDto employeeDto)
        {
            var employee = await GetEmployeeById(id);

            var updateDto = new EmployeeForUpdateDto
            {
                Name = employeeDto.Name ?? employee.Name,
                Age = employeeDto.Age ?? employee.Age,
                Position = employeeDto.Position ?? employee.Position,
                CompanyId = employeeDto.CompanyId ?? employee.CompanyId
            };

            await UpdateEmployee(id, updateDto);
        }

        public async Task DeleteEmployee(int id)
        {
            var employee = await GetEmployeeById(id);
            await _employeeRepository.DeleteEmployee(id);
        }
    }
} 