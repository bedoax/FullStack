using Business.Interfaces.Repository;
using Microsoft.EntityFrameworkCore;
using Models.DTOs.Role;
using Models.Entities;
namespace Data.Repositories
{
    public class RoleRepository : IRoleRepository
    {
        private  AppDbContext _context;

        public RoleRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddRole(CreateRoleDto role)
        {
            bool exists = await _context.Roles
                .AnyAsync(r => r.Name == role.Name);

            if (exists)
                throw new InvalidOperationException("Role already exists.");

            var newRole = new Role
            {
                Name = role.Name.Trim()
            };

            await _context.Roles.AddAsync(newRole);

    
        }

        public async Task DeleteRole(int roleId)
        {
            // see if you will use it or not
            if (await _context.Users.AnyAsync(u => u.RoleId == roleId))
            {
                throw new InvalidOperationException(
                    "Cannot delete role assigned to users.");
            }
            var role = await _context.Roles.FindAsync(roleId);

            if (role == null)
                throw new KeyNotFoundException("Role not found.");

            _context.Roles.Remove(role);
        }

        public async Task<RoleDto?> GetRoleById(int roleId)
        {
            return await _context.Roles
                .AsNoTracking()
                .Where(r => r.Id == roleId)
                .Select(r => new RoleDto
                {
                    Id = r.Id,
                    Name = r.Name
                })
                .FirstOrDefaultAsync();
        }

        public async Task<RoleDto?> GetRoleByName(string roleName)
        {
            var roleNameTrimLower = roleName.Trim().ToLower();
            return await _context.Roles
                .AsNoTracking()
                .Where(r => r.Name == roleNameTrimLower)
                .Select(r => new RoleDto
                {
                    Id = r.Id,
                    Name = r.Name
                })
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<RoleDto>> GetRoles()
        {
            return await _context.Roles
                .AsNoTracking()
                .OrderBy(r => r.Name)
                .Select(r => new RoleDto
                {
                    Id = r.Id,
                    Name = r.Name
                })
                .ToListAsync();
        }

        public async Task UpdateRole(UpdateRoleDto role)
        {
            var existingRole = await _context.Roles
                .FirstOrDefaultAsync(r => r.Id == role.Id);

            if (existingRole == null)
                throw new KeyNotFoundException("Role not found.");

            bool duplicateRole = await _context.Roles
                .AnyAsync(r =>
                    r.Id != role.Id &&
                    r.Name == role.Name);

            if (duplicateRole)
                throw new InvalidOperationException("Role already exists.");

            existingRole.Name = role.Name.Trim();

            await _context.SaveChangesAsync();
        }
    }
}


