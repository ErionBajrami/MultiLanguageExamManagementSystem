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
        private readonly QuestionsService _questionsService;

        public QuestionsController(QuestionsService questionsService)
        {
            _questionsService = questionsService;
        }



        #region Create



        [HttpPost("AddQuestion")]
        [Authorize(Roles = "Professor")]
        public async Task<ActionResult<int>> AddQuestion([FromBody] QuestionInsertDTO questionDto)
        {
            var question = await _questionsService.AddQuestions(questionDto.Text, questionDto.PossibleAnswers, questionDto.CorrectAnswer);

            return Ok(question);
        }
        #endregion


        #region Read


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
        #endregion


        #region Update



        [HttpPut("{questionId}")]
        [Authorize(Roles = "Professor")]
        public async Task<IActionResult> UpdateQuestion(int questionId, [FromBody] UpdateQuestionDTO updateQuestionDTO)
        {
            var updatedQuestion = await _questionsService.UpdateQuestion(
                questionId,
                updateQuestionDTO.QuestionText,
                updateQuestionDTO.PossibleAnswers,
                updateQuestionDTO.CorrectAnswer
            );

            return Ok(updatedQuestion);
        }



        #endregion


        #region Delete



        [HttpDelete("{questionId}")]
        [Authorize(Roles = "Professor")]
        public async Task<IActionResult> DeleteQuestion(int questionId)
        {
            await _questionsService.DeleteQuestion(questionId);
            return Ok("Question deleted successfully");
        }



        #endregion




    }
}
