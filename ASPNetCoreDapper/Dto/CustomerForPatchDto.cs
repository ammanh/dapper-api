using System.ComponentModel.DataAnnotations;

namespace ASPNetCoreDapper.Dto
{
    public class CustomerForPatchDto
    {
        [StringLength(100, ErrorMessage = "Name cannot be longer than 100 characters")]
        public string? Name { get; set; }

        [EmailAddress(ErrorMessage = "Invalid email format")]
        [StringLength(100, ErrorMessage = "Email cannot be longer than 100 characters")]
        public string? Email { get; set; }

        [Phone(ErrorMessage = "Invalid phone number format")]
        [StringLength(20, ErrorMessage = "Phone number cannot be longer than 20 characters")]
        public string? Phone { get; set; }
    }
} 