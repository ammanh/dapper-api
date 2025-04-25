using ASPNetCoreDapper.Entities;
using System.Threading.Tasks;

namespace appsettings.json.Contracts
{
    public interface IEmployeeRepository
    {
        Task<Company> GetCompanyByEmployeeId(int id);
        Task<Company> GetCompanyEmployeesMultipleResults(int id);
        Task<List<Company>> GetCompaniesEmployeesMultipleMapping();
    }
}
