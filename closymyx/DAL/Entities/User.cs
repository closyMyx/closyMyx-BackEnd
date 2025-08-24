using System.ComponentModel.DataAnnotations;

namespace closymyx.DAL.Entities
{
    public class User
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required, EmailAddress]
        public string Email { get; set; } = null!;

        [Required]
        public string PasswordHash { get; set; } = null!;

        public ICollection<Favorite> Favorites { get; set; } = new List<Favorite>();
         public ICollection<Top> Top { get; set; } = new List<Top>();
    }
}
