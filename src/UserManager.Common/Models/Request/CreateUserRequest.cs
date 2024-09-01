using System.ComponentModel.DataAnnotations;

namespace UserManager.Common.Models.Request
{
    public class CreateUserRequest
    {
        [Required]
        [MinLength(3), MaxLength(100)]
        [RegularExpression(@"^[a-zA-Z0-9]+$", ErrorMessage = "Username can only contain alphanumeric characters")]
        public string Username { get; set; }
        [Required]
        [MinLength(6), MaxLength(100)]
        public string Password { get; set; }
        [Required]
        [MaxLength(100)]
        [EmailAddress]
        public string Email { get; set; }
    }
}
