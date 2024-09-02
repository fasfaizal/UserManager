using System.ComponentModel.DataAnnotations;

namespace UserManager.Common.Models.Request
{
    public class UserDetailsRequest
    {
        [Required]
        [MaxLength(100)]
        public string FirstName { get; set; }

        [MaxLength(100)]
        public string LastName { get; set; }

        [MaxLength(20)]
        [RegularExpression(@"^\+?[0-9]{1,20}$", ErrorMessage = "Invalid phone number")]
        public string Phone { get; set; }

        [MaxLength(255)]
        public string Address { get; set; }

        public DateTime? DateOfBirth { get; set; }
    }
}
