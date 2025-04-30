using ASPNetCoreDapper.Dto;
using ASPNetCoreDapper.Entities;

namespace ASPNetCoreDapper.Services
{
    public interface IEmployeeService
    {
        Task<IEnumerable<Employee>> GetAllEmployees();
        Task<Employee> GetEmployeeById(int id);
        Task<Employee> CreateEmployee(EmployeeForCreationDto employeeDto);
        Task UpdateEmployee(int id, EmployeeForUpdateDto employeeDto);
        Task PatchEmployee(int id, EmployeeForPatchDto employeeDto);
        Task DeleteEmployee(int id);
    }
} 