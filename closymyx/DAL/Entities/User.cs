using System.ComponentModel.DataAnnotations;

namespace closymyx.DAL.Entities
{
    public class User
    {
        public Guid ID { get; set; } = Guid.NewGuid();

        [Required, EmailAddress]
        public string Email { get; set; } = null!;
        [Required]
        public string PasswordHash { get; set; } = null!;
    }
}