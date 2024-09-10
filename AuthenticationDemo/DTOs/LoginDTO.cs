using System.ComponentModel.DataAnnotations;

namespace AuthenticationDemo.DTOs
{
    public record LoginDTO
    {
        [Required]
        public string UserName { get; set; } = string.Empty;
        [Required]
        public string Password { get; set; } = string.Empty;
    }
}
