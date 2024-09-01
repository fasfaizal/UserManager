using System.ComponentModel.DataAnnotations;

namespace UserManager.Common.Models.Request
{
    public class LoginRequest
    {
        [Required]
        public string UsernameOrEmail { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
