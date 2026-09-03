using Models.DTOs.Auth;
using Models.DTOs.Pagination;
using Models.DTOs.Student;
using Models.DTOs.Teacher;
using Models.DTOs.User;
using Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Interfaces.Repository
{
    public interface IUserRepository
    {
        Task<IEnumerable<UserDto>> GetUsersAsync(
            int pageNumber,
            int pageSize);

        Task<UserDto> GetUserById(
            int userId);
        Task<UserEntity?> GetUserEntityById(int userId);
        Task<User?> GetUserByGoogleIdAsync(string googleId);
        Task<UserEntityWithRole?> GetUserEntityByUsername(string username);
        Task<User?> GetUserEntityByEmail(string email);
        Task<UserDto> GetUserByEmail(
            string email);

        Task<bool> IsEmailExists(
            string email);
        Task<User> AddUserAsync(
            UserCreateDto user);

        Task UpdateUser(
            UserUpdateDto user);

        Task DeleteUser(
            int userId);
        Task AssignRole(int userId, int roleId);
        Task ChangePassword(int userId, string hashedPassword);
        Task<IEnumerable<TeacherDto>> GetTeachers();
        Task Active(int userId);
        Task<PaginatedResult<StudentDto>> GetStudents(int page, int size);
    }
}
