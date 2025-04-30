using ASPNetCoreDapper.Dto;
using ASPNetCoreDapper.Entities;
using System.Threading.Tasks;

namespace ASPNetCoreDapper.Contracts
{
    public interface IEmployeeRepository
    {
        Task<IEnumerable<Employee>> GetEmployees();
        Task<Employee> GetEmployee(int id);
        Task<Employee> CreateEmployee(EmployeeForCreationDto employee);
        Task UpdateEmployee(int id, EmployeeForUpdateDto employee);
        Task DeleteEmployee(int id);
    }
}
