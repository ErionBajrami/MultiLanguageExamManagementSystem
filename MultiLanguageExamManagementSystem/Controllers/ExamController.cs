using Microsoft.AspNetCore.Mvc;
using MultiLanguageExamManagementSystem.Models.Dtos.Question;
using MultiLanguageExamManagementSystem.Models.Entities;
using MultiLanguageExamManagementSystem.Services;
using System.Collections.Generic;
using System.Threading.Tasks;
using MultiLanguageExamManagementSystem.Models.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using MultiLanguageExamManagementSystem.Models.Dtos.Exam;

namespace MultiLanguageExamManagementSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ExamController : ControllerBase
    {
        private readonly ExamService _examService;

        public ExamController(ExamService examService)
        {
            _examService = examService;
        }

        #region Read


        [HttpGet("GetAllExamDetail")]
        [Authorize(Roles = "Professor")]
        public async Task<ActionResult<List<ExamDetailsDTO>>> GetAllExamDetail()
        {
            var examDetails = await _examService.GetAllExamDetailsAsync();
            return Ok(examDetails);
        }

        [HttpGet("GetAllExamRequests")]
        [Authorize(Roles = "Professor")]
        public async Task<ActionResult<List<RequestExam>>> GetAllExamRequests()
        {
            var examRequests = _examService.GetExamRequests();
            return Ok(examRequests);
        }

        [HttpGet("GetApprovedExams")]
        public ActionResult<IEnumerable<Exam>> GetApprovedExams(int userId)
        {
            var approvedExams = _examService.GetApprovedExams(userId);
            return Ok(approvedExams);
        }

        [HttpGet("GetExamQuestions/{examId}")]
        [Authorize(Roles = "Professor")]
        public ActionResult<Exam> GetExamQuestions(int examId)
        {
            var exam = _examService.GetExamQuestions(examId);
            if (exam == null)
                return NotFound();
            return Ok(exam);
        }



        #endregion


        #region Create


        [HttpPost("AddExam")]
        [Authorize(Roles = "Professor")]
        public async Task<ActionResult> AddExam(AddExamDTO addExamDto)
        {
            var examId = await _examService.CreateExam(addExamDto.Title, addExamDto.StartTime, addExamDto.EndTime, addExamDto.ProfessorId);
            return Ok(examId);
        }


        [HttpPost("RequestExam")]
        public async Task<ActionResult> RequestExam(int userId, int examId)
        {
            await _examService.RequestExam(userId, examId);
            return Ok();
        }


        [HttpPost("SubmitExam")]
        public async Task<ActionResult<ExamResultDTO>> SubmitExamAsync(int userId, int examId, Dictionary<int, string> answers)
        {
            try
            {
                var examResultDTO = await _examService.SubmitExam(userId, examId, answers);
                return Ok(examResultDTO);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }


        #endregion


        #region Update



        [HttpPut("{examId}")]
        [Authorize(Roles = "Professor")]
        public async Task<IActionResult> UpdateExam(int examId, [FromBody] UpdateExamDTO updateExamDTO)
        {
            var updatedExam = await _examService.UpdateExam(
                examId,
                updateExamDTO.Title,
                updateExamDTO.StartTime,
                updateExamDTO.EndTime,
                updateExamDTO.ProfessorId
            );

            return Ok(updatedExam);
        }

        [HttpPost("ApproveRequest")]
        [Authorize(Roles = "Professor")]
        public ActionResult ApproveRequest(int requestId)
        {
            var result = _examService.ApproveRequest(requestId);
            if (result)
                return Ok();
            return BadRequest();
        }

        [HttpPost("RejectRequest")]
        [Authorize(Roles = "Professor")]
        public ActionResult RejectRequest(int requestId)
        {
            var result = _examService.RejectRequest(requestId);
            if (result)
                return Ok();
            return BadRequest();
        }




        #endregion


        #region Delete


        
        [HttpDelete("{examId}")]
        [Authorize(Roles = "Professor")]
        public async Task<IActionResult> DeleteExam(int examId)
        { 
            await _examService.DeleteExam(examId);
            return Ok("Exam deleted successfully");
            
        }



        #endregion
    }
}
