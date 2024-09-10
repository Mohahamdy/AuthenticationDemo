using System.ComponentModel.DataAnnotations;

namespace AuthenticationDemo.DTOs
{
    public record UserRoleDTO
    {
        [Required]
        public Guid UserId { get; set; }
        [Required]
        public Guid RoleId { get; set; }
    }
}
