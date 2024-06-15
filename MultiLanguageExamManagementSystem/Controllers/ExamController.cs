using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MultiLanguageExamManagementSystem.Data;
using MultiLanguageExamManagementSystem.Data.UnitOfWork;
using MultiLanguageExamManagementSystem.Models.Dtos;
using MultiLanguageExamManagementSystem.Models.Entities;
using MultiLanguageExamManagementSystem.Services;

namespace MultiLanguageExamManagementSystem.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ExamController : ControllerBase
{
    private readonly UnitOfWork _unitOfWork;
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly ExamService _examService;

    public ExamController(UnitOfWork unitOfWork, ApplicationDbContext context, IMapper mapper, ExamService examService)
    {
        _unitOfWork = unitOfWork;
        _context = context;
        _mapper = mapper;
        _examService = examService;
    }

    
    [HttpGet("GetAllExamsDetailed")]
    public async Task<List<Exam>> GetExamsDetailed()
    {
        var Exams = await _unitOfWork.Repository<Exam>()
            .GetAll()
            .Select( x => _mapper.Map<Exam>(x))
            .ToListAsync();

        return Exams;
    }
    
    [HttpGet("GetAllExams")]
    public async Task<ActionResult<List<AddExamDTO>>> GetAllExams()
    {
        var examRepository = _unitOfWork.Repository<Exam>();
        var exams = examRepository.GetAll();
    
        var addExams = exams.Select(exam => new AddExamDTO
        {
            ProfessorId = exam.ProfessorId,
            Title = exam.Title,
            StartTime = exam.StartTime,
            EndTime = exam.EndTime
        }).ToList();
    
        return Ok(addExams);
    }

    [HttpPost("AddExam")]
    public async Task AddExam(AddExamDTO addExamDto)
    {
        var addExam = _mapper.Map<Exam>(addExamDto);
        _unitOfWork.Repository<Exam>().Create(addExam);
        _unitOfWork.Complete();
    }

    [HttpPost("RequestExam")]
    public async Task RequestExam(int userId, int examId)
    {
        await _examService.RequestExam(userId, examId);
    }
}