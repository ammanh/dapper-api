IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'DapperASPNetCore')
BEGIN
    CREATE DATABASE DapperASPNetCore;
END
GO

USE DapperASPNetCore;
GO

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Companies')
BEGIN
    CREATE TABLE Companies (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        Name NVARCHAR(100) NOT NULL,
        Address NVARCHAR(200),
        Country NVARCHAR(50)
    );
END
GO 