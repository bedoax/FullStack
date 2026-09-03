using Models.DTOs.Role;

namespace Business.Interfaces.Repository
{
    public interface IRoleRepository
    {
        Task<IEnumerable<RoleDto>> GetRoles();

        Task<RoleDto> GetRoleById(
            int roleId);

        Task<RoleDto> GetRoleByName(
            string roleName);

        Task AddRole(
            CreateRoleDto role);

        Task UpdateRole(
            UpdateRoleDto role);

        Task DeleteRole(
            int roleId);
    }
}
