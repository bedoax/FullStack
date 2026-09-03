using Business.Interfaces.Repository;
using Models.DTOs.Attempt;
using Models.DTOs.Pagination;
using Models.DTOs.Question;
using Models.DTOs.Quiz;
using Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Services
{
   
    public class TeacherService : ITeacherService
    {
        private IUnitOfWork _unitOfWork;
        public TeacherService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public Task<IEnumerable<TeacherQuizDto>> GetMyQuizzes(int teacherId,CancellationToken ct)
        {
            return _unitOfWork.Quizzes.GetMyQuizzes(teacherId,ct);
        }
        public Task<List<TeacherAttemptStudentDto>> GetMyAttemptsByQuizId(int teacherId,int quizId)
        {
            return _unitOfWork.Attempts.GetMyAttemptsByQuizId(teacherId, quizId);
        }
        public Task<PaginatedResult<QuestionDto>> GetMyQuestions(int page, int size, int teacherId)
        {
            return _unitOfWork.Questions.GetMyQuestions(page, size, teacherId);
        }
        public Task<List<QuizStudentDto>> GetStudentsOfQuiz(int teacherId,int quizId)
        {
            return _unitOfWork.Attempts.GetStudentsOfQuiz(teacherId, quizId);
        }
        public async Task<QuizStatisticsDto> GetStaticsByQuizId(int teacherId,int quizId)
        {
            var students = await GetStudentsOfQuiz(teacherId, quizId);

            var totalStudents = students.Count;

            var passedStudents = students.Count(x => x.Passed == true);

            var averageScore = totalStudents == 0
                ? 0
                : students.Average(x => x.Score ?? 0);

            return new QuizStatisticsDto
            {
                TotalStudents = totalStudents,
                PassedStudents = passedStudents,
                FailedStudents = totalStudents - passedStudents,
                AverageScore = averageScore
            };
        }
        public  Task<TeacherDashboardDto> TeacherDashboard(int teacherId,CancellationToken ct)
        {
            return _unitOfWork.Quizzes.TeacherDashboard(teacherId,ct);
        }


    }
    }
