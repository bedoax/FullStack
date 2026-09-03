using Business.Interfaces;
using Business.Interfaces.Repository;
using Models.DTOs.Role;


namespace Business.Services
{
    public class RoleService : IRoleService
    {
        private IUnitOfWork _unitOfWork;
        public RoleService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task AddRole(CreateRoleDto role)
        {
            await _unitOfWork.Roles.AddRole(role);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteRole(int roleId)
        {
            await _unitOfWork.Roles.DeleteRole(roleId);
            await _unitOfWork.SaveChangesAsync();
        }

        public Task<IEnumerable<RoleDto>> GetAllRoles()
        {
            return _unitOfWork.Roles.GetRoles();
        }
        public Task<RoleDto> GetRoleByName(string roleName)
        {
            return _unitOfWork.Roles.GetRoleByName(roleName);
        }
        public Task<RoleDto> GetRoleById(int roleId)
        {
            return _unitOfWork.Roles.GetRoleById(roleId);
        }

        public async Task UpdateRole(UpdateRoleDto updateRoleDto)
        {
            await _unitOfWork.Roles.UpdateRole(updateRoleDto);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
