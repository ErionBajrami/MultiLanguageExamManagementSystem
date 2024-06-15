namespace MultiLanguageExamManagementSystem.Models.Dtos;

public class AddExamDTO
{
    public int ProfessorId { get; set; }
    public string Title { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
}