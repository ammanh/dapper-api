USE [DapperASPNetCore]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[ShowCompanyForProvidedEmployeeId] @Id int
AS
SELECT c.Id, c.Name, c.Address, c.Country
FROM Companies c JOIN Employees e ON c.Id = e.CompanyId
Where e.Id = @Id

GO