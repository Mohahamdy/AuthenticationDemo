using AuthenticationDemo.DTOs;
using AuthenticationDemo.Models;
using AuthenticationDemo.Utilies.APIResponse;

namespace AuthenticationDemo.Services
{
    public interface IAuthService
    {
        Task<APIResponseModel<string>> RegisterAsync(User _user);
        Task<APIResponseModel<string>> LoginAsync(LoginDTO _loginDo);
        Task<APIResponseModel<string>> AssignUserToRoleAsync(UserRoleDTO _userRoleDTO);
        Task<APIResponseModel<IQueryable<RegisterDTO>>> GetAllUsersAsync();
    }
}
