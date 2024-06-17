namespace MultiLanguageExamManagementSystem.Models.Dtos.Exam
{
    public class ExamSubmissionDTO
    {
        public int ExamId { get; set; }
        public int UserId { get; set; }
        public Dictionary<int, string> Answers { get; set; }
    }
}
