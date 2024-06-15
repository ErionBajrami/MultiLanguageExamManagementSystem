using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MultiLanguageExamManagementSystem.Data.UnitOfWork;
using MultiLanguageExamManagementSystem.Models.Dtos.Question;
using MultiLanguageExamManagementSystem.Services;

namespace MultiLanguageExamManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuestionsController : ControllerBase
    {
        private readonly UnitOfWork _unitOfWork;
        private readonly ExamService _examService;

        public QuestionsController(UnitOfWork unitOfWork, ExamService examService)
        {
            _unitOfWork = unitOfWork;
            _examService = examService;

        }


        [HttpGet]
        public async Task<IEnumerable<QuestionRetrieveDTO>> GetAllQuestions()
        {
            var questions = await _examService.GetAllQuestions();

            return questions;
        }

        [HttpPut]
        public async Task<ActionResult<int>> AddQuestion([FromBody] QuestionInsertDTO questionDto)
        {
            var question = await _examService.AddQuestions(questionDto.Text, questionDto.CorrectAnswer);

            return Ok(question);
        }

        
    }
}
