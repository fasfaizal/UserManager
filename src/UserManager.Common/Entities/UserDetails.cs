using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UserManager.Repo.Entities
{
    public class UserDetails : BaseEntity
    {
        [Key]
        [ForeignKey("User")]
        public int UserId { get; set; }
        [Required]
        [MaxLength(100)]
        public string FirstName { get; set; }
        [MaxLength(100)]
        public string LastName { get; set; }
        [MaxLength(20)]
        public string Phone { get; set; }
        [MaxLength(255)]
        public string Address { get; set; }
        public DateTime? DateOfBirth { get; set; }
    }
}
