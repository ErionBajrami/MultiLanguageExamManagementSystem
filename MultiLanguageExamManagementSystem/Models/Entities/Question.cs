public class Question
{
    public int QuestionId { get; set; }
    public string Content { get; set; }
    public ICollection<Exam> Exams { get; set; }
}