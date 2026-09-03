using Business.Interfaces;
using Business.Interfaces.Repository;
using Microsoft.AspNetCore.Identity;
using Models.DTOs.Pagination;
using Models.DTOs.Student;
using Models.DTOs.Teacher;
using Models.DTOs.User;
using Models.Entities;


namespace Business.Services
{
    public class UserService : IUserService
    {
        private IUnitOfWork _unitOfWork;
        private IPasswordService _passwordService;
        public UserService(IUnitOfWork unitOfWork,IPasswordService passwordService)
        {
            _unitOfWork = unitOfWork;
            _passwordService = passwordService;
        }
        public async Task AssignRole(int userId, int roleId)
        {
            await _unitOfWork.Users.AssignRole(userId, roleId);
            
        }

        public async Task CreateUser(UserCreateDto dto)
        {
            // we will make the hashing for password 
            // there here for creating teacher or admin
            dto.Password =_passwordService.HashPassword(dto.Password);
           
            await _unitOfWork.Users.AddUserAsync(dto);

            await _unitOfWork.SaveChangesAsync();
           
        }
        public  async Task DeleteUser(int userId)
        {
            await _unitOfWork.Users.DeleteUser(userId);
        }
        public async Task ActiveUser(int userId)
        {
            await _unitOfWork.Users.Active(userId);
        }

        public  Task<IEnumerable<UserDto>> GetAllUsers(int pageNumber,int pageSize)
        {
            return  _unitOfWork.Users.GetUsersAsync(pageNumber,
             pageSize);
        }

        public  Task<UserDto> GetUserByEmail(string email)
        {
            return  _unitOfWork.Users.GetUserByEmail(email);
        }

        public  Task<UserDto> GetUserById(int userId)
        {
            return  _unitOfWork.Users.GetUserById(userId);
        }

        public async Task UpdateUser(UserUpdateDto dto)
        {
            await  _unitOfWork.Users.UpdateUser(dto);
            await _unitOfWork.SaveChangesAsync();
        }
        public async Task<IEnumerable<TeacherDto>> GetTeachersAsync()
        {
           return await _unitOfWork.Users.GetTeachers();
        }
        public async Task<PaginatedResult<StudentDto>> GetStudentsAsync(int page, int size)
        {
            return await _unitOfWork.Users.GetStudents(page,size);
        }
    }
   
    public class PasswordService : IPasswordService
    {
        private readonly PasswordHasher<User> _hasher = new();

        public string HashPassword(string password)
        {
            return _hasher.HashPassword(null!, password);
        }

        public bool VerifyPassword(string providedPassword, string hashedPassword)
        {
            var result = _hasher.VerifyHashedPassword(
                null!,
                hashedPassword,
                providedPassword);

            return result == PasswordVerificationResult.Success
                || result == PasswordVerificationResult.SuccessRehashNeeded;
        }
    }
}
