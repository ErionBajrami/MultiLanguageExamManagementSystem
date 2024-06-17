namespace MultiLanguageExamManagementSystem.Models.Dtos.Exam
{
    public class UpdateExamDTO
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public int ProfessorId { get; set; }
    }
}
