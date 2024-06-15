using MultiLanguageExamManagementSystem.Models.Entities;

public class Exam
{
    public int ExamId { get; set; }
    public string Title { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    
    public int ProfessorId { get; set; }
    public User Professor { get; set; }
    public bool ApprovedRequest { get; set; } = false;
    public ICollection<ExamQuestion> ExamQuestions { get; set; } 
}