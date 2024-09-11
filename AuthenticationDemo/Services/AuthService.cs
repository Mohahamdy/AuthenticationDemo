using AuthenticationDemo.DTOs;
using AuthenticationDemo.Models;
using AuthenticationDemo.Utilies.APIResponse;
using AuthenticationDemo.Utilies.Token;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Net.Sockets;

namespace AuthenticationDemo.Services
{
    public class AuthService(AuthDbContext _context, IJwtToken _token) : IAuthService
    {
        const string roleName = "User";

        public async Task<APIResponseModel<string>> AssignUserToRoleAsync(UserRoleDTO _userRoleDTO)
        {
            var user = await _context.Users.FindAsync(_userRoleDTO.UserId);
            if (user is null)
                return APIResponseFactory<string>.Failure("User or role is not existed", "Failed to assign!", HttpStatusCode.BadRequest);

            var role = await _context.Roles.FindAsync(_userRoleDTO.RoleId);
            if (role is null)
                return APIResponseFactory<string>.Failure("User or role is not existed", "Failed to assign!", HttpStatusCode.BadRequest); ;

            try
            {
                await _context.UsersRoles.AddAsync(new UserRoles { RoleId = _userRoleDTO.RoleId, UserId = _userRoleDTO.UserId });
                await _context.SaveChangesAsync();

                return APIResponseFactory<string>.Success($"Assign {user.UserName} to role {role.Name} successfuly","Assigned successfuly",HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return APIResponseFactory<string>.Failure($"Somethin went wrong while processing database and inner excepion is {ex.InnerException}", "Internal server error!", HttpStatusCode.InternalServerError); ;

            }
        }

        public Task<APIResponseModel<IQueryable<RegisterDTO>>> GetAllUsersAsync()
        {
            var users = _context.Users.Select(u => new RegisterDTO
            {
                FirstName = u.FirstName,
                LastName = u.LastName,
                Email = u.Email,
                Password = u.Password,
                UserName = u.UserName
            });
            return Task.FromResult(
                APIResponseFactory<IQueryable<RegisterDTO>>.Success(users, "Get all users", HttpStatusCode.OK));
        }

        public async Task<APIResponseModel<string>> LoginAsync(LoginDTO _loginDo)
        {
            var user = await _context.Users
                .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
                .FirstOrDefaultAsync(u => u.UserName == _loginDo.UserName);

            if (user is null ||
                PasswordNotMatched(user.Password,_loginDo.Password))
                return APIResponseFactory<string>.Failure("Username Or password is incorrect","Failed to log in!",HttpStatusCode.BadRequest);

            return APIResponseFactory<string>.Success(await _token.Generate(user),"Loged in successfuly",HttpStatusCode.OK);
        }

        public async Task<APIResponseModel<string>> RegisterAsync(User _user)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u=> u.Email == _user.Email);

            if (user is not null)
                return APIResponseFactory<string>.Failure("Email is already existed","Failed to register!",HttpStatusCode.BadRequest);

            var User = await _context.Users.FirstOrDefaultAsync(u=> u.UserName == _user.UserName);
            if (User is not null)
                return APIResponseFactory<string>.Failure("Username is already existed", "Failed to register!", HttpStatusCode.BadRequest);

            var role = await _context.Roles.FirstOrDefaultAsync(r => r.Name == roleName);

            try
            {
                await _context.Users.AddAsync(_user);
                await _context.UsersRoles.AddAsync(new UserRoles { RoleId = role.Id , UserId = _user.Id});
                await _context.SaveChangesAsync();
                
                return APIResponseFactory<string>.Success(await _token.Generate(_user), "Regiserd successfuly", HttpStatusCode.OK); ;
            }
            catch (Exception ex)
            {
                return APIResponseFactory<string>.Failure($"Somethin went wrong while processing database and inner excepion is {ex.InnerException}", "Internal server error!", HttpStatusCode.InternalServerError); ;
            }
        }



        private bool PasswordNotMatched(string userPassword,string loginPassword)
        {
            var hasher = new PasswordHasher<User>();
            var verificationResult = hasher.VerifyHashedPassword(null, userPassword, loginPassword);

            return verificationResult == PasswordVerificationResult.Failed;
        }
    }
}
