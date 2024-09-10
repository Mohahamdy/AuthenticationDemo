using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace AuthenticationDemo.Models
{
    public class User
    {
        [Key]
        public Guid Id { get; set; }

        [MaxLength(50)]
        public string FirstName { get; set; } = string.Empty;

        [MaxLength(50)]
        public string LastName { get; set; } = string.Empty;

        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        public string UserName { get; set; } = string.Empty;

        public string Password { get; private set; } = string.Empty;

        public List<UserRoles> UserRoles { get; set; }

        public void SetPassword(string password)
        {
            var hasher = new PasswordHasher<User>();
            Password = hasher.HashPassword(this, password);
        }
    }
}

