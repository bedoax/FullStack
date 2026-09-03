using Models.DTOs.Role;


namespace Business.Interfaces
{
    public interface IRoleService
    {
        /*
                Create Role
                Update Role
                Delete Role
                Get Role
                Get All Roles
         */
        Task AddRole(CreateRoleDto role);
        Task UpdateRole(UpdateRoleDto updateRoleDto);
        Task<RoleDto> GetRoleById(int roleId);
        Task DeleteRole(int roleId);
        Task<IEnumerable<RoleDto>> GetAllRoles();
        Task<RoleDto> GetRoleByName(string roleName);
    }


}
