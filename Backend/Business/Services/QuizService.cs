using Business.Interfaces;
using Business.Interfaces.Repository;
using Models.DTOs.Question;
using Models.DTOs.Quiz;
using Models.DTOs.QuizQuestionDto;
using Models.DTOs.StudentTopicPerformance;

namespace Business.Services
{
    public class QuizService : IQuizService
    {
        private IUnitOfWork _unitOfWork;
        public QuizService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task AddQuestionToQuiz(CreateQuizQuestionDto createQuizQuetion)
        {

            await _unitOfWork.QuizQuestions.AddQuizQuestion(createQuizQuetion);
            await _unitOfWork.SaveChangesAsync();
        }
        public async Task AddQuestionsToQuiz(int userId,CreateQuizQuestionsDto createQuizQuestions)
        {

            await _unitOfWork.QuizQuestions.AddQuizQuestions(userId, createQuizQuestions);
            await _unitOfWork.SaveChangesAsync();
        }
        public async Task<int> AddRandomQuestionsToQuizAsync(int teacherId, int quizId, int? topicId, int count)
        {
            return await _unitOfWork.QuizQuestions.AddRandomQuestionsToQuizAsync(teacherId, quizId, topicId, count);
        }
        public async Task CreateQuiz(CreateQuizDto quizCreateDto)
        {
            await _unitOfWork.Quizzes.AddQuiz(quizCreateDto);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteQuiz(int quizId,int userId,string userRoleName)
        {
            await _unitOfWork.Quizzes.DeleteQuiz(quizId,userId,userRoleName);
            await _unitOfWork.SaveChangesAsync();
        }

        public Task<IEnumerable<QuizDto>> GetAllQuizs(CancellationToken ct)
        {
            return _unitOfWork.Quizzes.GetAllQuizzes(ct);
        }
        public Task<IEnumerable<QuizDto>> GetAllDraftQuizs(CancellationToken ct)
        {
            return _unitOfWork.Quizzes.GetAllDraftQuizzes(ct);
        }
        public Task<QuizDto?> GetPublishedQuizById(int id)
        {
           return _unitOfWork.Quizzes.GetPublishedQuizById(id);
        }
        public Task<QuizDto?> GetDraftQuizById(int id)
        {
            return _unitOfWork.Quizzes.GetDraftQuizById(id);
        }
        public Task<IEnumerable<StudentQuizDto>> GetMyQuizzes(int userId,CancellationToken ct)
        {
            return _unitOfWork.Quizzes.GetAllQuizzesByUserId(userId,ct);
        }
        public Task<IEnumerable<QuestionDetailsDto>> GetQuizQuestions(int quizId)
        {
            return _unitOfWork.QuizQuestions.GetQuizQuestions(quizId);
        }

        public async Task RemoveQuestionFromQuiz(int quizId, int questionId)
        {
            await  _unitOfWork.QuizQuestions.RemoveQuizQuestion(quizId, questionId);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task UpdateQuiz(int id,QuizUpdateDto updateQuizDto,int userId,string userRoleName)
        {   

            await _unitOfWork.Quizzes.UpdateQuiz(id,updateQuizDto,userId,userRoleName);
            await _unitOfWork.SaveChangesAsync();

        }
        public   Task<IEnumerable<QuestionDetailsDtoForStudent>> GetQuizQuestionsForStudent(int quizId)
        {
            return  _unitOfWork.QuizQuestions.GetQuizQuestionsForStudent(quizId);
        }
        public Task<IEnumerable<LeaderboardDto>> LeaderboardByQuizId(int quizId,CancellationToken ct)
        {
            return _unitOfWork.Quizzes.LeaderboardByQuizId(quizId,ct);
        }

        /*
              GetAllQuizzes
GetMyQuizzes
GetAllDraftQuizzes
GetAllQuestionsOfTheQuiz
LeaderboardByQuizId
         
         */
    }
}
