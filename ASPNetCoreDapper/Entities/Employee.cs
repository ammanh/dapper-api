﻿namespace ASPNetCoreDapper.Entities
{
    public class Employee
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public string Position { get; set; }
        public int CompanyId { get; set; }
        public Company Company { get; set; }
    }
}
