using ASPNetCoreDapper.Contracts;
using ASPNetCoreDapper.Context;
using ASPNetCoreDapper.Dto;
using ASPNetCoreDapper.Entities;
using Dapper;
using System.Data;

namespace ASPNetCoreDapper.Repository
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly DapperContext _context;

        public EmployeeRepository(DapperContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Employee>> GetEmployees()
        {
            var query = "SELECT * FROM Employees";
            using (var connection = _context.CreateConnection())
            {
                var employees = await connection.QueryAsync<Employee>(query);
                return employees.ToList();
            }
        }

        public async Task<Employee> GetEmployee(int id)
        {
            var query = "SELECT * FROM Employees WHERE Id = @Id";
            using (var connection = _context.CreateConnection())
            {
                var employee = await connection.QuerySingleOrDefaultAsync<Employee>(query, new { id });
                return employee;
            }
        }

        public async Task<Employee> CreateEmployee(EmployeeForCreationDto employee)
        {
            var query = @"INSERT INTO Employees (Name, Age, Position, CompanyId)
                        VALUES (@Name, @Age, @Position, @CompanyId);
                        SELECT CAST(SCOPE_IDENTITY() as int)";

            var parameters = new DynamicParameters();
            parameters.Add("Name", employee.Name, DbType.String);
            parameters.Add("Age", employee.Age, DbType.Int32);
            parameters.Add("Position", employee.Position, DbType.String);
            parameters.Add("CompanyId", employee.CompanyId, DbType.Int32);

            using (var connection = _context.CreateConnection())
            {
                var id = await connection.QuerySingleAsync<int>(query, parameters);

                var createdEmployee = new Employee
                {
                    Id = id,
                    Name = employee.Name,
                    Age = employee.Age,
                    Position = employee.Position,
                    CompanyId = employee.CompanyId
                };

                return createdEmployee;
            }
        }

        public async Task UpdateEmployee(int id, EmployeeForUpdateDto employee)
        {
            var query = "UPDATE Employees SET Name = @Name, Age = @Age, Position = @Position, CompanyId = @CompanyId WHERE Id = @Id";

            var parameters = new DynamicParameters();
            parameters.Add("Id", id, DbType.Int32);
            parameters.Add("Name", employee.Name, DbType.String);
            parameters.Add("Age", employee.Age, DbType.Int32);
            parameters.Add("Position", employee.Position, DbType.String);
            parameters.Add("CompanyId", employee.CompanyId, DbType.Int32);

            using (var connection = _context.CreateConnection())
            {
                await connection.ExecuteAsync(query, parameters);
            }
        }

        public async Task DeleteEmployee(int id)
        {
            var query = "DELETE FROM Employees WHERE Id = @Id";
            using (var connection = _context.CreateConnection())
            {
                await connection.ExecuteAsync(query, new { id });
            }
        }

        public async Task<Company> GetCompanyByEmployeeId(int id)
        {
            var procedureName = "ShowCompanyForProvidedEmployeeId";
            var parameters = new DynamicParameters();
            parameters.Add("Id", id, DbType.Int32, ParameterDirection.Input);

            using (var connection = _context.CreateConnection())
            {
                var company = await connection.QueryFirstOrDefaultAsync<Company>
                    (procedureName, parameters, commandType: CommandType.StoredProcedure);

                return company;
            }
        }

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

                return company;
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
    }
}
