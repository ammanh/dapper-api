using ASPNetCoreDapper.Contracts;
using ASPNetCoreDapper.Dto;
using ASPNetCoreDapper.Entities;

namespace ASPNetCoreDapper.Services
{
    public class CompanyService : ICompanyService
    {
        private readonly ICompanyRepository _companyRepository;

        public CompanyService(ICompanyRepository companyRepository)
        {
            _companyRepository = companyRepository;
        }

        public async Task<IEnumerable<Company>> GetAllCompanies()
        {
            return await _companyRepository.GetCompanies();
        }

        public async Task<Company> GetCompanyById(int id)
        {
            var company = await _companyRepository.GetCompany(id);
            if (company == null)
                throw new KeyNotFoundException($"Company with ID {id} not found");
            return company;
        }

        public async Task<Company> CreateCompany(CompanyForCreationDto companyDto)
        {
            if (string.IsNullOrWhiteSpace(companyDto.Name))
                throw new ArgumentException("Company name is required");

            return await _companyRepository.CreateCompany(companyDto);
        }

        public async Task UpdateCompany(int id, CompanyForUpdateDto companyDto)
        {
            if (string.IsNullOrWhiteSpace(companyDto.Name))
                throw new ArgumentException("Company name is required");

            var company = await GetCompanyById(id);
            await _companyRepository.UpdateCompany(id, companyDto);
        }

        public async Task PatchCompany(int id, CompanyForPatchDto companyDto)
        {
            var company = await GetCompanyById(id);

            var updateDto = new CompanyForUpdateDto
            {
                Name = companyDto.Name ?? company.Name,
                Address = companyDto.Address ?? company.Address,
                Country = companyDto.Country ?? company.Country
            };

            await _companyRepository.UpdateCompany(id, updateDto);
        }

        public async Task DeleteCompany(int id)
        {
            var company = await GetCompanyById(id);
            await _companyRepository.DeleteCompany(id);
        }

        public async Task<Company> GetCompanyWithEmployees(int id)
        {
            var company = await _companyRepository.GetCompanyEmployeesMultipleResults(id);
            if (company == null)
                throw new KeyNotFoundException($"Company with ID {id} not found");
            return company;
        }

        public async Task<IEnumerable<Company>> GetCompaniesWithEmployees()
        {
            return await _companyRepository.GetCompaniesEmployeesMultipleMapping();
        }
    }
} 