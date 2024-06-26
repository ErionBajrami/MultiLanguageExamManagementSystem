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
    private readonly IAuthService _authService;

    public UserController(IAuthService authService)
    {
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
    public async Task<IActionResult> AddUser(User user)
    {
        await _authService.AddUser(user);

        return Ok("User created successfully");
    }



    #endregion

    #region Read



    [HttpGet("GetAllUsers")]
    [Authorize(Roles = "Professor")]
    public async Task<List<User>> GetAllUsers()
    {
        return await _authService.GetAllUsers();
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


    [HttpPut("{userId}")]
    public async Task<IActionResult> UpdateUser(int userId, [FromBody] UpdateUserDTO updateUserDto)
    {
        var updatedUser = _authService.UpdateUser(userId, updateUserDto.Username, updateUserDto.Password);
        return Ok(updatedUser);
    }



    #endregion

    #region Delete

    [HttpDelete("{userId}")]
    public async Task<IActionResult> DeleteUser(int userId)
    {
        await _authService.DeleteUser(userId);
        return Ok("User deleted succesfully");
    }

    #endregion

}