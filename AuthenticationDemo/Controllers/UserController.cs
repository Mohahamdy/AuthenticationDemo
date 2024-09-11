using AuthenticationDemo.DTOs;
using AuthenticationDemo.Models;
using AuthenticationDemo.Services;
using AuthenticationDemo.Utilies.APIResponse;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Reflection.Metadata.Ecma335;

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
                return BadRequest(APIResponseFactory<string>.Success(ModelState.ToString()!, "Inputs validation", HttpStatusCode.BadRequest));

            User _user = new User()
            {
                FirstName = _registerDTO.FirstName,
                LastName = _registerDTO.LastName,
                Email = _registerDTO.Email,
                UserName = _registerDTO.UserName,
            };
            _user.SetPassword(_registerDTO.Password);

            var result = await _authService.RegisterAsync(_user);

            return CheckStatusCode<string>(result);
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> LoginAsync([FromBody] LoginDTO _loginDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(APIResponseFactory<string>.Success(ModelState.ToString()!, "Inputs validation",HttpStatusCode.BadRequest));

            var result = await _authService.LoginAsync(_loginDTO);

            return CheckStatusCode<string>(result);
        }

        [HttpPost]
        [Route("AssignRoleToUser")]
        public async Task<IActionResult> AssignRoleToUserAsync([FromBody] UserRoleDTO _userRoleDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(APIResponseFactory<string>.Success(ModelState.ToString()!, "Inputs validation",HttpStatusCode.BadRequest));

            var result = await _authService.AssignUserToRoleAsync(_userRoleDTO);

            return CheckStatusCode<string>(result);
        }

        [HttpGet]
        [Authorize(Roles = "Developer,Admin")]
        public async Task<IActionResult> GetAllUsersAsync()
        {
            var result = await _authService.GetAllUsersAsync();

            return CheckStatusCode<IQueryable<RegisterDTO>>(result);
        }

        private IActionResult CheckStatusCode<T>(APIResponseModel<T> result) where T : class
        {

            if (result.StatusCode == HttpStatusCode.OK)
                return Ok(result);

            if (result.StatusCode == HttpStatusCode.BadRequest)
                return BadRequest(result);

            return StatusCode(StatusCodes.Status500InternalServerError, result);
        }
    }
}
