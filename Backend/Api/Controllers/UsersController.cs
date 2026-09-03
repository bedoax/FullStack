using Business.Helper;
using Business.Interfaces;
using Business.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models.DTOs.User;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private IUserService _userService;
        private IRoleService _roleService;
        public UsersController(IUserService userService,IRoleService roleService)
        {
            _userService = userService;
            _roleService = roleService;
        }
        [Authorize(Roles =RolesConstants.Admin)]
        [HttpPost("admins")]
        public async Task<IActionResult> CreateAdmin(CreateUserDto dto)
        {


            var roleAdmin = await _roleService.GetRoleByName(RolesConstants.Admin);
            var userAdminRole = new UserCreateDto
            {
                Email = dto.Email,
                Password = dto.Password,
                Username = dto.Username,
                RoleId = roleAdmin.Id
            };
            await _userService.CreateUser(userAdminRole);

            return StatusCode(StatusCodes.Status201Created);
        }
        [Authorize(Roles = RolesConstants.Admin)]

        [HttpPost("teachers")]
        public async Task<IActionResult> CreateTeacher(CreateUserDto dto)
        {
            var roleTeacher = await _roleService.GetRoleByName(RolesConstants.Teacher);
            var userTeacherRole = new UserCreateDto
            {
                Email = dto.Email,
                Password = dto.Password,
                Username = dto.Username,
                RoleId = roleTeacher.Id
            };
            await _userService.CreateUser(userTeacherRole);
            return StatusCode(StatusCodes.Status201Created);
        }

        [Authorize(Roles = RolesConstants.Admin)]
        [HttpGet]
        public async Task<IActionResult> GetUsers(int page = 1, int pageSize = 10)
        {
            var users = await _userService.GetAllUsers(page, pageSize);
            return Ok(users);
        }

        [Authorize(Roles = RolesConstants.Admin)]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(int id)
        {
            var user = await _userService.GetUserById(id);
            if (user == null)
                return NotFound();
            return Ok(user);
        }

        [Authorize(Roles = RolesConstants.AdminOrTeacherOrStudent)]
        [HttpGet("me")]
        public async Task<IActionResult> GetUserById()
        {
            var userId = User.GetUserId();
            var user = await _userService.GetUserById(userId);
            if (user == null)
                return NotFound();
            return Ok(user);
        }
        [Authorize(Roles =RolesConstants.Admin)]
        [HttpGet("teachers")]
        public async Task<IActionResult> GetTeachers()
        {
            var result = await _userService.GetTeachersAsync();
            return Ok(result);
        }
        [Authorize(Roles = RolesConstants.Admin)]
        [HttpGet("students")]
        public async Task<IActionResult>GetStudents([FromQuery]int page = 1 , [FromQuery] int pageSize = 10)
        {
            var result = await _userService.GetStudentsAsync(page, pageSize);
            return Ok(result);
        }

        [Authorize(Roles = RolesConstants.AdminOrTeacherOrStudent)]
        [HttpPut("me")]
        public async Task<IActionResult> UpdateUser([FromBody] UserUpdateRequest userDto)
        {
            var userId = User.GetUserId();
            var userUpdateDto = new UserUpdateDto
            {
                Id = userId,
                Username = userDto.Username,
                Email = userDto.Email
            };
            await _userService.UpdateUser(userUpdateDto);
            return NoContent();
        }

        [Authorize(Roles = RolesConstants.Student)]
        [HttpDelete("me")]
        // i added a global filter for get the exist user , not deleted user too 
        public async Task<IActionResult> DeleteUser()
        {
            var userId = User.GetUserId();
            await _userService.DeleteUser(userId);
            return NoContent();
        }

        [Authorize(Roles = RolesConstants.Admin)]
        [HttpDelete("{userId:int}/delete")]
        public async Task<IActionResult> DeleteUser(int userId)
        {
            await _userService.DeleteUser(userId);
            return NoContent();
        }
        [Authorize (Roles = RolesConstants.Admin)]
        [HttpPut("{userId:int}/active")]
        public async Task<IActionResult> ActiveUser(int userId)
        {
            await _userService.ActiveUser(userId);
            return NoContent();
        }
        /*
         maybe new feature 
         POST /users/{id}/ban
POST /users/{id}/unban
         
         */
    }
}
