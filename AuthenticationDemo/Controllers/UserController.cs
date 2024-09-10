using AuthenticationDemo.DTOs;
using AuthenticationDemo.Models;
using AuthenticationDemo.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuthenticationDemo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController(IAuthService _authService): ControllerBase
    {
        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> RgisterAsync([FromBody] RegisterDTO _registerDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            User _user = new User()
            {
                FirstName = _registerDTO.FirstName,
                LastName = _registerDTO.LastName,
                Email = _registerDTO.Email,
                UserName = _registerDTO.UserName,
            };
            _user.SetPassword(_registerDTO.Password);

            return Ok(await _authService.RegisterAsync(_user));
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> LoginAsync([FromBody] LoginDTO _loginDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(await _authService.LoginAsync(_loginDTO));
        }

        [HttpPost]
        [Route("AssignRoleToUser")]
        public async Task<IActionResult> LoginAsync([FromBody] UserRoleDTO _userRoleDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(await _authService.AssignUserToRoleAsync(_userRoleDTO));
        }

        [HttpGet]
        [Authorize(Roles = "Developer,Admin")]
        public async Task<ActionResult<IQueryable<RegisterDTO>>> GetAllUsersAsync()
        {
            return Ok(await _authService.GetAllUsersAsync());
        }
    }
}
