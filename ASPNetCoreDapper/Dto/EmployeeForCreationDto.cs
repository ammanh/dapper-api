using System.ComponentModel.DataAnnotations;

namespace ASPNetCoreDapper.Dto
{
    public class EmployeeForCreationDto
    {
        [Required(ErrorMessage = "Employee name is required")]
        [StringLength(100, ErrorMessage = "Name cannot be longer than 100 characters")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Age is required")]
        [Range(18, 100, ErrorMessage = "Age must be between 18 and 100")]
        public int Age { get; set; }

        [Required(ErrorMessage = "Position is required")]
        [StringLength(50, ErrorMessage = "Position cannot be longer than 50 characters")]
        public string Position { get; set; }

        [Required(ErrorMessage = "Company ID is required")]
        public int CompanyId { get; set; }
    }
} 