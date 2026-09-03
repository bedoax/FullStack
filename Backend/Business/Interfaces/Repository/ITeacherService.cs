using Models.DTOs.Attempt;
using Models.DTOs.Pagination;
using Models.DTOs.Question;
using Models.DTOs.Quiz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Interfaces.Repository
{
    public interface ITeacherService
    {
        Task<IEnumerable<TeacherQuizDto>> GetMyQuizzes(int teacherId, CancellationToken ct);
        Task<List<TeacherAttemptStudentDto>> GetMyAttemptsByQuizId(int teacherId, int quizId);
        Task<PaginatedResult<QuestionDto>> GetMyQuestions(int page, int size, int teacherId);
        Task<List<QuizStudentDto>> GetStudentsOfQuiz(int teacherId,  int quizId);
         Task<QuizStatisticsDto> GetStaticsByQuizId(int teacherId, int quizId);
        Task<TeacherDashboardDto> TeacherDashboard(int teacherId,CancellationToken ct);
    }
}
