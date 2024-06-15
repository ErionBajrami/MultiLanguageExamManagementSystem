namespace MultiLanguageExamManagementSystem.Models.Entities;

public class RequestExam
{
    public int RequestExamId { get; set; }
    public int StudentId { get; set; }
    public User Student { get; set; }
    public int ExamId { get; set; }
    public Exam Exam { get; set; }
}