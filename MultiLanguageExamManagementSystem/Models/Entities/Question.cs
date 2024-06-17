public class Question
{
    public int QuestionId { get; set; }
    public string QuestionText { get; set; }
    public string possibleAnswers { get; set; }
    public string? GivenAnswer { get; set; }

    public string CorrectAnswer { get; set; }

    public ICollection<ExamQuestion> ExamQuestions { get; set; }
}