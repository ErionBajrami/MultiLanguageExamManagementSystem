using MultiLanguageExamManagementSystem.Models.Dtos.Exam;
using MultiLanguageExamManagementSystem.Models.Dtos;
using MultiLanguageExamManagementSystem.Models.Entities;

namespace MultiLanguageExamManagementSystem.Services.IServices
{
    public interface IExamService
    {
        #region Read
        IEnumerable<Exam> GetAvailableExams();
        Task<List<ExamDetailsDTO>> GetAllExamDetailsAsync();
        Exam GetExam(int id);
        IEnumerable<RequestExam> GetExamRequests();
        ExamQuestionsDTO GetExamQuestions(int examId);
        IEnumerable<Exam> GetApprovedExams(int userId);
        #endregion

        #region Create
        Task<int> CreateExam(string title, DateTime startTime, DateTime endTime, int professorId);
        Task RequestExam(int userId, int examId);
        Task<ExamResultDTO> SubmitExam(int userId, int examId, Dictionary<int, string> answers);
        #endregion

        #region Update
        bool ApproveRequest(int requestId);
        bool RejectRequest(int requestId);
        #endregion


        #region Delete
        Task DeleteExam(int examId);
        #endregion
    }
}
