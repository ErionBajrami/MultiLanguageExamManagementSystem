namespace MultiLanguageExamManagementSystem.Models.Dtos.Question
{
    public class UpdateQuestionDTO
    {
        public int ID { get; set; }
        public string QuestionText { get; set; }
        public string PossibleAnswers { get; set; }
        public string CorrectAnswer { get; set; }
    }
}
