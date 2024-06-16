using MultiLanguageExamManagementSystem.Models.Dtos.Question;

namespace MultiLanguageExamManagementSystem.Models.Dtos;

public class ExamDetailsDTO
{
    public int ExamId { get; set; }
    public string Title { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public int ProfessorId { get; set; }
    public string ProfessorName { get; set; }
    public bool ApprovedRequest { get; set; }
    public List<QuestionDetailsDTO> ExamQuestions { get; set; }
}