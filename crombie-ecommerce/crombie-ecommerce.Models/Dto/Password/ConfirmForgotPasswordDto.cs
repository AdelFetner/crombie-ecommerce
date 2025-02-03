using System.ComponentModel.DataAnnotations;

namespace crombie_ecommerce.Models.Dto.Password
{
    public class ConfirmForgotPasswordDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [MinLength(8)]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&-])[A-Za-z\d@$!%*?&]{8,}$",
            ErrorMessage = "Password must have at least 8 characters, a UpperCase character, numbers and a special character.")]
        public string NewPassword { get; set; }

        [Required]
        public string ConfirmationCode { get; set; }
    }
}
