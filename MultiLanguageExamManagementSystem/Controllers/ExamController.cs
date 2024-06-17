using Microsoft.AspNetCore.Mvc;
using MultiLanguageExamManagementSystem.Models.Dtos.Question;
using MultiLanguageExamManagementSystem.Models.Entities;
using MultiLanguageExamManagementSystem.Services;
using System.Collections.Generic;
using System.Threading.Tasks;
using MultiLanguageExamManagementSystem.Models.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

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

        [HttpGet("GetAllExamDetail")]
        public async Task<ActionResult<List<ExamDetailsDTO>>> GetAllExamDetail()
        {
            var examDetails = await _examService.GetAllExamDetailsAsync();
            return Ok(examDetails);
        }


        [HttpGet("GetAllExamRequests")]
        public async Task<ActionResult<List<RequestExam>>> GetAllExamRequests()
        {
            var examRequests = _examService.GetExamRequests();
            return Ok(examRequests);
        }


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

        [HttpPost("ApproveRequest")]
        public ActionResult ApproveRequest(int requestId)
        {
            var result = _examService.ApproveRequest(requestId);
            if (result)
                return Ok();
            return BadRequest();
        }

        [HttpPost("RejectRequest")]
        public ActionResult RejectRequest(int requestId)
        {
            var result = _examService.RejectRequest(requestId);
            if (result)
                return Ok();
            return BadRequest();
        }

        [HttpGet("GetApprovedExams")]
        public ActionResult<IEnumerable<Exam>> GetApprovedExams(int userId)
        {
            var approvedExams = _examService.GetApprovedExams(userId);
            return Ok(approvedExams);
        }

        [HttpGet("GetExamQuestions/{examId}")]
        public ActionResult<Exam> GetExamQuestions(int examId)
        {
            var exam = _examService.GetExamQuestions(examId);
            if (exam == null)
                return NotFound();
            return Ok(exam);
        }
    }
}
