using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MultiLanguageExamManagementSystem.Data.UnitOfWork;
using MultiLanguageExamManagementSystem.Models.Dtos.User;
using MultiLanguageExamManagementSystem.Services;
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

    #region Create



    [HttpPost("signup")]
    public IActionResult SignUp([FromBody] SignUpDTO signUpDto)
    {
        _authService.SignUp(signUpDto);
        return Ok("User created successfully");
    }


    [HttpPost("AddUser")]
    [Authorize(Roles = "Professor")]
    public async Task AddUser(User user)
    {
        var AddUser = _mapper.Map<User>(user);
        _unitOfWork.Repository<User>().Create(AddUser);
        _unitOfWork.Complete();
    }



    #endregion

    #region Read



    [HttpGet("GetAllUsers")]
    [Authorize(Roles = "Professor")]
    public async Task<List<User>> GetUsers()
    {
        var users = await _unitOfWork.Repository<User>()
            .GetAll()
            .ToListAsync();

        return users;
    }


    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginDTO loginDto)
    {
        var response = _authService.Authenticate(loginDto);

        if (response == null)
            return NotFound();

        return Ok(response);
    }



    #endregion

    #region Update

    #endregion

    #region Delete



    #endregion

}