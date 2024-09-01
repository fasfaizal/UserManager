using System.ComponentModel.DataAnnotations;

namespace UserManager.Repo.Entities
{
    public class User : BaseEntity
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(100)]
        public string Username { get; set; }
        [Required]
        [MaxLength(255)]
        public string Email { get; set; }
        [Required]
        public byte[] PasswordHash { get; set; }
        [Required]
        public byte[] PasswordSalt { get; set; }
        public UserDetails UserDetails { get; set; }
    }
}
