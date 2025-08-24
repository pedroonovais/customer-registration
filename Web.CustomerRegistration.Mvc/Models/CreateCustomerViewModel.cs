using System.ComponentModel.DataAnnotations;

namespace Web.CustomerRegistration.Mvc.Models
{
    public sealed class CreateCustomerViewModel
    {
        [Required]
        public string Name { get; set; } = string.Empty;

        [Required, EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string Occupation { get; set; } = string.Empty;
    }
}
