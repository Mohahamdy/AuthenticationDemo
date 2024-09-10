using AuthenticationDemo.DTOs;
using AuthenticationDemo.Models;

namespace AuthenticationDemo.Services
{
    public interface IAuthService
    {
        Task<string> RegisterAsync(User _user);
        Task<string> LoginAsync(LoginDTO _loginDo);
        Task<string> AssignUserToRoleAsync(UserRoleDTO _userRoleDTO);
        Task<IQueryable<RegisterDTO>> GetAllUsersAsync();
    }
}
