using appsettings.json.Entities;

public interface ICompanyRepository
{
    public Task<IEnumerable<Company>> GetCompanies();
}