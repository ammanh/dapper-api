using ASPNetCoreDapper.Dto;
using ASPNetCoreDapper.Entities;

namespace ASPNetCoreDapper.Services
{
    public interface ICompanyService
    {
        Task<IEnumerable<Company>> GetAllCompanies();
        Task<Company> GetCompanyById(int id);
        Task<Company> CreateCompany(CompanyForCreationDto companyDto);
        Task UpdateCompany(int id, CompanyForUpdateDto companyDto);
        Task PatchCompany(int id, CompanyForPatchDto companyDto);
        Task DeleteCompany(int id);
        Task<Company> GetCompanyWithEmployees(int id);
        Task<IEnumerable<Company>> GetCompaniesWithEmployees();
    }
} 