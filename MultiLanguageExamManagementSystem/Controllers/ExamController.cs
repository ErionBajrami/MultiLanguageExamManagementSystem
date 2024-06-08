using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MultiLanguageExamManagementSystem.Data;
using MultiLanguageExamManagementSystem.Data.UnitOfWork;

namespace MultiLanguageExamManagementSystem.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ExamController : ControllerBase
{
    private readonly UnitOfWork _unitOfWork;
    private readonly ApplicationDbContext _context;

    public ExamController(UnitOfWork unitOfWork, ApplicationDbContext context)
    {
        _unitOfWork = unitOfWork;
        _context = context;
    }

    [HttpGet("GetAllExams")]
    public async Task<ActionResult<List<Exam>>> GetAllExams()
    {
        var exams = await _context.Exams.ToListAsync();
        return Ok(exams);
    }

    [HttpPost("AddExam")]
    public IActionResult AddExam([FromBody] Exam exam)
    {
        _context.Add(exam);
        _context.SaveChanges();

        return Ok(exam);
    }
}