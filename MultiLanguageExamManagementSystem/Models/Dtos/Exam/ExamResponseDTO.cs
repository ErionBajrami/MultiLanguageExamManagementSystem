using MultiLanguageExamManagementSystem.Models.Dtos.Question;
// using MultiLanguageExamManagementSystem.Models.Entities.User(<--- error here);

// Error	CS0234	The type or namespace name 'User' does not exist in the namespace 'MultiLanguageExamManagementSystem.Models.Entities' (are you missing an assembly reference?)
// ??????????????????????????????????


namespace MultiLanguageExamManagementSystem.Models.Dtos.Exam
{
    public class ExamResponseDTO
    {
       // public User MyProperty { get; set; }
        public ICollection<QuestionRetrieveDTO> Questions { get; set; }
    }
}
