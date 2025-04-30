using System.ComponentModel.DataAnnotations;

namespace ASPNetCoreDapper.Dto
{
    public class CompanyForCreationDto
    {
        [Required(ErrorMessage = "Company name is required")]
        [StringLength(100, ErrorMessage = "Name cannot be longer than 100 characters")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Address is required")]
        [StringLength(200, ErrorMessage = "Address cannot be longer than 200 characters")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Country is required")]
        [StringLength(50, ErrorMessage = "Country cannot be longer than 50 characters")]
        public string Country { get; set; }
    }
}
