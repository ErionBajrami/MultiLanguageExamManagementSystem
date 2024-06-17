using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MultiLanguageExamManagementSystem.Data.UnitOfWork;
using MultiLanguageExamManagementSystem.Models.Dtos.User;
using MultiLanguageExamManagementSystem.Services.IServices;

namespace MultiLanguageExamManagementSystem.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly UnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IAuthService _authService;

    public UserController(UnitOfWork unitOfWork, IMapper mapper, IAuthService authService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _authService = authService;
    }



    [HttpGet("GetAllUsers")]
    public async Task<List<User>> GetUsers()
    {
        var users = await _unitOfWork.Repository<User>()
            .GetAll()
            .ToListAsync();

        return users;
    }

    [HttpPost("AddUser")]
    public async Task AddUser(User user)
    {
        var AddUser = _mapper.Map<User>(user);
        _unitOfWork.Repository<User>().Create(AddUser);
        _unitOfWork.Complete();
    }

    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginDTO loginDto)
    {
        var response = _authService.Authenticate(loginDto);

        if (response == null)
            return NotFound();

        return Ok(response);
    }

    [HttpPost("signup")]
    public IActionResult SignUp([FromBody] SignUpDTO signUpDto)
    {
        try
        {
            _authService.SignUp(signUpDto);
            return Ok("User created successfully");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}