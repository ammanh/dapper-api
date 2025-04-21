using appsettings.json.Entities;
using ASPNetCoreDapper.Context;
using Dapper;
using System.Data;

public class CompanyRepository : ICompanyRepository
{
    private readonly DapperContext _context;

    public CompanyRepository(DapperContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Company>> GetCompanies()
    {
        var query = "SELECT * FROM Companies";
        using (var connection = _context.CreateConnection())
        {
            var companies = await connection.QueryAsync<Company>(query);
            return companies.ToList();
        }
    }
}
