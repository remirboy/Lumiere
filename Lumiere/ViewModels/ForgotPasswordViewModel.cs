using System.ComponentModel.DataAnnotations;

namespace Lumiere.ViewModels
{
    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
