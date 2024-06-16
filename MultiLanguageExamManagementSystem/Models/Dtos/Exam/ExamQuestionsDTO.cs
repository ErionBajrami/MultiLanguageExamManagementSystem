using MultiLanguageExamManagementSystem.Models.Dtos.Question;

namespace MultiLanguageExamManagementSystem.Models.Dtos;

public class ExamQuestionsDTO
{
    public string Title { get; set; }
    public List<QuestionRetrieveDTO> ExamQuestions { get; set; }
}