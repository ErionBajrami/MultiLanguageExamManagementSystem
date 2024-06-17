namespace MultiLanguageExamManagementSystem.Models.Dtos.Exam
{
    public class ExamResultDTO
    {
        public int UserId { get; set; }
        public int ExamId { get; set; }
        public int TotalQuestions { get; set; }
        public int CorrectAnswers { get; set; }
        public double Score { get; set; }
    }
}
