using MultiLanguageExamManagementSystem.Models.Dtos.Question;

namespace MultiLanguageExamManagementSystem.Services.IServices
{
    public interface IQuestionService
    {
        #region Create


        Task<Question> AddQuestions(string questionText, string possibleAnswers, string correctAnswer);

        #endregion

        #region Read



        Task<IEnumerable<QuestionRetrieveDTO>> GetAllQuestionsUser();

        Task<IEnumerable<QuestionInsertDTO>> GetAllQuestionsProfessor();



        #endregion

        #region Update


        Task<Question> UpdateQuestion(int questionId, string questionText, string possibleAnswers, string correctAnswer);


        #endregion

        #region Delete


        Task DeleteQuestion(int questionId);


        #endregion
    }
}
