using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MultiLanguageExamManagementSystem.Data.UnitOfWork;

namespace MultiLanguageExamManagementSystem.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController
{
    private readonly UnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UserController(UnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }



    [HttpGet("GetAllUsers")]
    public async Task<List<User>> GetUsers()
    {
        var users = await _unitOfWork.Repository<User>()
            .GetAll()
            .Select( x => _mapper.Map<User>(x))
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
}