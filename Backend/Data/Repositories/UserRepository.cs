using Business.Interfaces.Repository;
using Microsoft.EntityFrameworkCore;
using Models.DTOs.Pagination;
using Models.DTOs.Student;
using Models.DTOs.Teacher;
using Models.DTOs.User;
using Models.Entities;
namespace Data.Repositories
{
    public class UserRepository : IUserRepository
    {
        private AppDbContext _context;
        public UserRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<User> AddUserAsync(UserCreateDto user)
        {
            bool emailExists = await IsEmailExists(user.Email);

            if (emailExists)
                throw new InvalidOperationException("Email already exists.");

            bool usernameExists = await _context.Users
                .AnyAsync(u => u.Username.ToLower() == user.Username.Trim().ToLower());

            if (usernameExists)
                throw new InvalidOperationException("Username already exists.");


            var newUser = new User
            {
                Username = user.Username.Trim(),
                Email = user.Email.Trim().ToLower(),
                Password = user.Password, 
                RoleId = user.RoleId,
                CreatedAt = DateTime.UtcNow,
                GoogleId = user.GoogleId,
            };

            await _context.Users.AddAsync(newUser);
            return newUser;
        }

        public async Task DeleteUser(int userId)
        {
            // Soft delete user to preserve attempts and related historical data
            var rows = await _context.Users
                .Where(u => u.Id == userId)
                .ExecuteUpdateAsync(s =>
                    s.SetProperty(u => u.IsDeleted, true));

            if (rows == 0)
                throw new KeyNotFoundException("User not found.");

        }
        public async Task Active(int userId)
        {
            var rows = await _context.Users
                 .IgnoreQueryFilters()
                 .Where(u => u.Id == userId)
                 .ExecuteUpdateAsync(s =>
                 s.SetProperty(u => u.IsDeleted, false));

            if (rows == 0)
                throw new KeyNotFoundException("User not found.");
        }
        public async Task<User?> GetUserByGoogleIdAsync(string googleId)
        {
            return await _context.Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(
                    u => u.GoogleId == googleId &&
                         !u.IsDeleted);
        }
        public async Task<User?> GetUserEntityByEmail(string email)
        {
            return await _context.Users.Include(u=> u.Role)
                .Where(u => u.Email == email && !u.IsDeleted)
                .FirstOrDefaultAsync();
        }
        public async Task<UserDto?> GetUserByEmail(string email)
        {
            return await _context.Users
                .Where(u => u.Email == email && !u.IsDeleted)
                .Select(u => new UserDto
                {
                    Id = u.Id,
                    Username = u.Username,
                    Email = u.Email,
                    RoleId = u.RoleId,
                    CreatedAt = u.CreatedAt,
                    RoleName = u.Role.Name
                })
                .FirstOrDefaultAsync();
        }

        public async Task<UserDto?> GetUserById(int userId)
        {
            return await _context.Users
                .Where(u => u.Id == userId && !u.IsDeleted)
                .Select(u => new UserDto
                {
                    Id = u.Id,
                    Username = u.Username,
                    Email = u.Email,
                    RoleId = u.RoleId,
                    CreatedAt = u.CreatedAt,
                    RoleName = u.Role.Name


                })
                .FirstOrDefaultAsync();
        }
        public async Task<UserEntity?> GetUserEntityById(int userId)
        {
            return await _context.Users
                .Where(u => u.Id == userId && !u.IsDeleted)
                .Select(u => new UserEntity
                {
                    Id = u.Id,
                    Username = u.Username,
                    Email = u.Email,
                    RoleId = u.RoleId,
                    CreatedAt = u.CreatedAt,
                    Password =u.Password

                })
                .FirstOrDefaultAsync();
        }
        public async Task<UserEntityWithRole?> GetUserEntityByUsername(string username)
        {
            var usernameTrim = username.Trim().ToLower(); 
            return await _context.Users
                .Where(u => u.Username.ToLower() == usernameTrim && !u.IsDeleted)
                .Include(u=>u.Role)
                .Select(u => new UserEntityWithRole
                {
                    Id = u.Id,
                    Username = u.Username,
                    Email = u.Email,
                    RoleId = u.RoleId,
                    CreatedAt = u.CreatedAt,
                    Password = u.Password,
                    RoleName = u.Role.Name

                })
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<UserDto>> GetUsersAsync(
            int pageNumber,
            int pageSize)
        {
            return await _context.Users
                .Where(u=> !u.IsDeleted)
                .OrderBy(u => u.Id)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(u => new UserDto
                {
                    Id = u.Id,
                    Username = u.Username,
                    Email = u.Email,
                    RoleId = u.RoleId,
                    CreatedAt = u.CreatedAt
                })
                .ToListAsync();
        }


        public async Task<IEnumerable<TeacherDto>> GetTeachers()
        {
            int teacherRoleId = 3;
            return await _context.Users
                    .IgnoreQueryFilters()
                    .AsNoTracking()
                    .Where(x => x.RoleId == teacherRoleId)
                    .Select(x => new TeacherDto
                    {
                        Id = x.Id,
                        Name = x.Username,
                        Email = x.Email,
                        IsDeleted = x.IsDeleted,
                        CreatedAt = x.CreatedAt,
                        Questions = x.CreatedQuestions.Count,
                        Quizzes = x.CreatedQuizzes.Count
                    })
                    .ToListAsync();
        }
        public async Task<PaginatedResult<StudentDto>> GetStudents(int page, int size)
        {
            int studentRoleId = 2;

            var query = _context.Users
                .IgnoreQueryFilters()
                .AsNoTracking()
                .Where(x => x.RoleId == studentRoleId);

            int totalCount = await query.CountAsync();

            var students = await query
                .OrderByDescending(x => x.CreatedAt)
                .Skip((page - 1) * size)
                .Take(size)
                .Select(x => new StudentDto
                {
                    Id = x.Id,
                    Name = x.Username,
                    Email = x.Email,
                    IsDeleted = x.IsDeleted,
                    CreatedAt = x.CreatedAt,
                })
                .ToListAsync();

            int totalPages = (int)Math.Ceiling((double)totalCount / size);

            return new PaginatedResult<StudentDto>
            {
                Items = students,
                Size = size,
                Page = page,
                TotalCount = totalCount,
                TotalPages = totalPages
            };
        }

        public async Task<bool> IsEmailExists(string email)
        {
            return await _context.Users
                .AnyAsync(u => u.Email.ToLower() == email.Trim().ToLower() && !u.IsDeleted);
        }

        public async Task UpdateUser(UserUpdateDto user)
        {
            var existingUser = await _context.Users
                .FirstOrDefaultAsync(u => u.Id == user.Id);

            if (existingUser == null)
                throw new KeyNotFoundException("User not found.");

            var username = user.Username.Trim();
            var email = user.Email.Trim();

            bool usernameExists = await _context.Users
                .AnyAsync(u =>
                    u.Id != user.Id &&
                    u.Username == username);

            if (usernameExists)
                throw new InvalidOperationException("Username already exists.");
            var emailExists = await _context.Users.AnyAsync(u => u.Id != user.Id &&u.Email == email);

            if (emailExists)
                throw new InvalidOperationException("Email already exists.");
            existingUser.Username = username;
            existingUser.Email = email;
        }
        public async Task AssignRole(int userId,int roleId)
        {
            if (userId <= 0 || roleId <= 0)
                throw new KeyNotFoundException("User or role not valid");
            var isUserExist = await _context.Users.AnyAsync(u => u.Id == userId);
            if (!isUserExist)
                throw new KeyNotFoundException("Key not found");
            var isRoleExist = await _context.Roles.AnyAsync(u => u.Id == roleId);
            if (!isRoleExist)
                throw new KeyNotFoundException("Key not found");
            await _context.Users.Where(u=>u.Id == userId)
                .ExecuteUpdateAsync(s => 
                s.SetProperty(u => u.RoleId, roleId));
        }
        public async Task ChangePassword(int userId,string hashedPassword)
        {

            await _context.Users.Where(x=>x.Id == userId).ExecuteUpdateAsync(x => x.SetProperty(u => u.Password, hashedPassword));
        }
    }
}


