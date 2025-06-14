﻿using ASPNetCoreDapper.Contracts;
using ASPNetCoreDapper.Context;
using ASPNetCoreDapper.Dto;
using ASPNetCoreDapper.Entities;
using Dapper;
using System.Data;


namespace ASPNetCoreDapper.Repository
{
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

        public async Task<Company> GetCompany(int id)
        {
            var query = "SELECT * FROM Companies WHERE ID = @Id";
            using (var connection = _context.CreateConnection())
            {
                var company = await connection.QuerySingleOrDefaultAsync<Company>(query, new { id });
                return company ?? throw new KeyNotFoundException($"Company with ID {id} not found");
            }
        }

        public async Task<Company> CreateCompany(CompanyForCreationDto company)
        {
            var query = @"INSERT INTO Companies (Name, Address, Country)
                        VALUES (@Name, @Address, @Country);
                        SELECT CAST(SCOPE_IDENTITY() as int);";

            var parameters = new DynamicParameters();
            parameters.Add("Name", company.Name, DbType.String);
            parameters.Add("Address", company.Address, DbType.String);
            parameters.Add("Country", company.Country, DbType.String);

            using (var connection = _context.CreateConnection())
            {
                var id = await connection.QuerySingleAsync<int>(query, parameters);

                var createdCompany = new Company
                {
                    Id = id,
                    Name = company.Name,
                    Address = company.Address,
                    Country = company.Country
                };

                return createdCompany;
            }
        }

        public async Task UpdateCompany(int id, CompanyForUpdateDto company)
        {
            var query = "UPDATE Companies SET Name = @Name, Address = @Address, Country = @Country WHERE Id = @Id";

            var parameters = new DynamicParameters();
            parameters.Add("Id", id, DbType.Int32);
            parameters.Add("Name", company.Name, DbType.String);
            parameters.Add("Address", company.Address, DbType.String);
            parameters.Add("Country", company.Country, DbType.String);

            using (var connection = _context.CreateConnection())
            {
                await connection.ExecuteAsync(query, parameters);
            }
        }

        public async Task DeleteCompany(int id)
        {
            using (var connection = _context.CreateConnection())
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    await connection.ExecuteAsync("DELETE FROM Employees WHERE CompanyId = @Id", new { id }, transaction);
                    await connection.ExecuteAsync("DELETE FROM Companies WHERE Id = @Id", new { id }, transaction);
                    transaction.Commit();
                }
            }
        }

        //public async Task<Company> GetCompanyByEmployeeId(int id)
        //{
        //    var procedureName = "ShowCompanyForProvidedEmployeeId";
        //    var parameters = new DynamicParameters();
        //    parameters.Add("Id", id, DbType.Int32, ParameterDirection.Input);

        //    using (var connection = _context.CreateConnection())
        //    {
        //        var company = await connection.QueryFirstOrDefaultAsync<Company>
        //            (procedureName, parameters, commandType: CommandType.StoredProcedure);

        //        return company;
        //    }
        //}

        public async Task<Company> GetCompanyEmployeesMultipleResults(int id)
        {
            var query = "SELECT * FROM Companies WHERE Id = @Id;" +
                        "SELECT * FROM Employees WHERE CompanyId = @Id";

            using (var connection = _context.CreateConnection())
            using (var multi = await connection.QueryMultipleAsync(query, new { id }))
            {
                var company = await multi.ReadSingleOrDefaultAsync<Company>();
                if (company != null)
                    company.Employees = (await multi.ReadAsync<Employee>()).ToList();
                return company; /*?? throw new KeyNotFoundException($"Company with ID {id} not found");*/
            }
        }

        public async Task<List<Company>> GetCompaniesEmployeesMultipleMapping()
        {
            var query = "SELECT * FROM Companies c JOIN Employees e ON c.Id = e.CompanyId";
            using (var connection = _context.CreateConnection())
            {
                var companyDict = new Dictionary<int, Company>();
                var companies = await connection.QueryAsync<Company, Employee, Company>(
                    query, (company, employee) =>
                    {
                        if (!companyDict.TryGetValue(company.Id, out var currentCompany))
                        {
                            currentCompany = company;
                            companyDict.Add(currentCompany.Id, currentCompany);
                        }
                        currentCompany.Employees.Add(employee);
                        return currentCompany;
                    }
                );
                return companies.Distinct().ToList();
            }
        }

        public async Task CreateMultipleCompanies(List<CompanyForCreationDto> companies)
        {
            var query = "INSERT INTO Companies (Name, Address, Country) VALUES (@Name, @Address, @Country)";

            using (var connection = _context.CreateConnection())
            {
                connection.Open();

                using (var transaction = connection.BeginTransaction())
                {
                    foreach (var company in companies)
                    {
                        var parameters = new DynamicParameters();
                        parameters.Add("Name", company.Name, DbType.String);
                        parameters.Add("Address", company.Address, DbType.String);
                        parameters.Add("Country", company.Country, DbType.String);

                        await connection.ExecuteAsync(query, parameters, transaction: transaction);
                    }

                    transaction.Commit();
                }
            }
        }
    }
}






