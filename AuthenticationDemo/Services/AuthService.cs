using AuthenticationDemo.DTOs;
using AuthenticationDemo.Models;
using AuthenticationDemo.Utilies.Token;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AuthenticationDemo.Services
{
    public class AuthService(AuthDbContext _context, IJwtToken _token) : IAuthService
    {
        const string roleName = "User";

        public async Task<string> AssignUserToRoleAsync(UserRoleDTO _userRoleDTO)
        {
            var user = await _context.Users.FindAsync(_userRoleDTO.UserId);
            if (user is null)
                return "User is not existed";

            var role = await _context.Roles.FindAsync(_userRoleDTO.RoleId);
            if (role is null)
                return "Role is not existed";

            try
            {
                await _context.UsersRoles.AddAsync(new UserRoles { RoleId = _userRoleDTO.RoleId, UserId = _userRoleDTO.UserId });
                await _context.SaveChangesAsync();

                return $"Assign {user.UserName} to role {role.Name} successfuly";
            }
            catch (Exception ex)
            {
                return $"Somethin went wrong while processing database and inner excepion is {ex.InnerException}";
            }
        }

        public Task<IQueryable<RegisterDTO>> GetAllUsersAsync()
        {
            return Task.FromResult(
                    _context.Users.Select(u=> new RegisterDTO
                    {
                        FirstName = u.FirstName,
                        LastName = u.LastName,
                        Email = u.Email,
                        Password = u.Password,
                        UserName = u.UserName
                    })
                );
        }

        public async Task<string> LoginAsync(LoginDTO _loginDo)
        {
            var user = await _context.Users
                .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
                .FirstOrDefaultAsync(u => u.UserName == _loginDo.UserName);

            if (user is null ||
                IsPasswordNotMatched(user.Password,_loginDo.Password))
                return "Username Or password is incorrect";

            return await _token.Generate(user);
        }

        public async Task<string> RegisterAsync(User _user)
        {
            

            var user = await _context.Users.FirstOrDefaultAsync(u=> u.Email == _user.Email);

            if (user is not null)
                return "Email is already existed";

            var User = await _context.Users.FirstOrDefaultAsync(u=> u.UserName == _user.UserName);
            if (User is not null)
                return "Username is already existed";

            var role = await _context.Roles.FirstOrDefaultAsync(r => r.Name == roleName);

            try
            {
                await _context.Users.AddAsync(_user);
                await _context.UsersRoles.AddAsync(new UserRoles { RoleId = role.Id , UserId = _user.Id});
                await _context.SaveChangesAsync();
                
                return await _token.Generate(_user);
            }
            catch (Exception ex)
            {
                return $"Somethin went wrong while processing database and inner excepion is {ex.InnerException}";
            }
        }



        private bool IsPasswordNotMatched(string userPassword,string loginPassword)
        {
            var hasher = new PasswordHasher<User>();
            var verificationResult = hasher.VerifyHashedPassword(null, userPassword, loginPassword);

            return verificationResult == PasswordVerificationResult.Failed;
        }
    }
}
