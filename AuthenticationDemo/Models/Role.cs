using System.ComponentModel.DataAnnotations;

namespace AuthenticationDemo.Models
{
    public class Role
    {
        [Key]
        public Guid Id { get; set; }
        [MaxLength(50)]
        public string Name { get; set; } = string.Empty;
        public List<UserRoles> UserRoles { get; set; }
    }
}
