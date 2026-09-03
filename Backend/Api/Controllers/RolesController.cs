using Business.Helper;
using Business.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models.DTOs.Role;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        /*
         GET /roles
POST /roles
PUT /roles/{id}
DELETE /roles/{id}
         */
        private IRoleService _roleService;
        public RolesController(IRoleService roleService)
        {
            _roleService = roleService;
        }
        
        [Authorize(Roles = RolesConstants.Admin)]
        [HttpGet]
        public async Task<IActionResult> GetAllRoles()
        {
            var roles = await _roleService.GetAllRoles();
            return Ok(roles);
        }
        
        [Authorize(Roles = RolesConstants.Admin)]
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetRoleById(int id)
        {
            var role = await _roleService.GetRoleById(id);
            if (role == null)
            {
                return NotFound();
            }
            return Ok(role);
        }
        
        [Authorize(Roles = RolesConstants.Admin)]
        [HttpPost]
        public async Task<IActionResult> CreateRole([FromBody] CreateRoleDto roleDto)
        {
            await _roleService.AddRole(roleDto);
            return Created();
        }
        
        [Authorize(Roles = RolesConstants.Admin)]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRole(int id, [FromBody] RoleUpdateRquest roleDto)
        {
            var updateRoleDto = new UpdateRoleDto
            {
                Id = id,
                Name = roleDto.Name
            };
            await _roleService.UpdateRole(updateRoleDto);
            return NoContent();
        }
        
        [Authorize(Roles = RolesConstants.Admin)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRole(int id)
        {
            await _roleService.DeleteRole(id);
            return NoContent();
        }
    }
}
