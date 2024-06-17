namespace MultiLanguageExamManagementSystem.Models.Entities
{
    public class ExamResult
    {
        public int ExamResultId { get; set; }
        public int UserId { get; set; }
        public int ExamId { get; set; }
        public double Score { get; set; }
        public int TotalQuestions { get; set; }
        public int CorrectAnswers { get; set; }
        public DateTime SubmissionDate { get; set; }

        public User User { get; set; }
        public Exam Exam { get; set; }

        public ExamResult()
        {
            SubmissionDate = DateTime.UtcNow;
        }
    }
}
