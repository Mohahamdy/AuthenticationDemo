using System.ComponentModel.DataAnnotations.Schema;

namespace AuthenticationDemo.Models
{
    public class UserRoles
    {
        [ForeignKey(nameof(User))]
        public Guid UserId { get; set; }
        [ForeignKey(nameof(Role))] 
        public Guid RoleId { get; set; }
        public User? User { get; set; }
        public Role? Role { get; set; }
    }
}
