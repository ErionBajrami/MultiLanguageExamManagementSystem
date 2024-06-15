namespace MultiLanguageExamManagementSystem.Models.Entities;

public class RequestExam
{
    public int RequestExamId { get; set; }
    public int UserId { get; set; }
    public int ExamId { get; set; }
    public RequestStatus Status { get; set; }
    public DateTime RequestDate { get; set; }

    public User User { get; set; }
    public Exam Exam { get; set; }
}