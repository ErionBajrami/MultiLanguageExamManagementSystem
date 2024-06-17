using Microsoft.AspNetCore.Authorization;
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
        private readonly QuestionsService _questionsService;

        public QuestionsController(UnitOfWork unitOfWork, ExamService examService, QuestionsService questionsService)
        {
            _unitOfWork = unitOfWork;
            _examService = examService;
            _questionsService = questionsService;
        }


        [HttpGet("GetAllQuestionsUser")]
        public async Task<IEnumerable<QuestionRetrieveDTO>> GetAllQuestionsUser()
        {
            var questions = await _questionsService.GetAllQuestionsUser();

            return questions;
        }

        [HttpGet("GetAllQuestionsProfessor")]
        [Authorize(Roles = "Professor")]
        public async Task<IEnumerable<QuestionInsertDTO>> GetAllQuestionsProfessor()
        {
            var questions = await _questionsService.GetAllQuestionsProfessor();

            return questions;
        }

        [HttpPost("AddQuestion")]
        public async Task<ActionResult<int>> AddQuestion([FromBody] QuestionInsertDTO questionDto)
        {
            var question = await _questionsService.AddQuestions(questionDto.Text, questionDto.PossibleAnswers, questionDto.CorrectAnswer);

            return Ok(question);
        }

        
    }
}
