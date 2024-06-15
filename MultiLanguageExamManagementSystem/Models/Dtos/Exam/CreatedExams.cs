namespace MultiLanguageExamManagementSystem.Models.Dtos;

public class CreatedExams
{
    public User Professor { get; set; }
    public string Title { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
}