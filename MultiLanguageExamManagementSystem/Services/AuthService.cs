using AutoMapper;
using LifeEcommerce.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MultiLanguageExamManagementSystem.Data.UnitOfWork;
using MultiLanguageExamManagementSystem.Models.Dtos;
using MultiLanguageExamManagementSystem.Models.Dtos.User;
using MultiLanguageExamManagementSystem.Services.IServices;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MultiLanguageExamManagementSystem.Services
{
    public class AuthService : IAuthService
    {

        private readonly IConfiguration _configuration;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public AuthService(IConfiguration configuration, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _configuration = configuration;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        #region Create



        public void SignUp(SignUpDTO signUp)
        {
            var existingUser = _unitOfWork.Repository<User>().GetByCondition(u => u.Username == signUp.Username).FirstOrDefault();
            if (existingUser != null)
                throw new Exception("User already exists");

            var newUser = new User
            {
                Username = signUp.Username,
                Password = HelperMethods.HashPassword(signUp.Password),
                Role = signUp.Role
            };

            _unitOfWork.Repository<User>().Create(newUser);
            _unitOfWork.Complete();
        }


        public async Task AddUser(User user)
        {
            var AddUser = _mapper.Map<User>(user);
            _unitOfWork.Repository<User>().Create(AddUser);
            _unitOfWork.Complete();
        }


        #endregion

        #region Read

        public async Task<List<User>> GetAllUsers()
        {
            var users = await _unitOfWork.Repository<User>()
                .GetAll()
                .ToListAsync();

            return users;
        }

        public AuthResponseDTO Authenticate(LoginDTO login)
        {
            var user = _unitOfWork.Repository<User>().GetByCondition(u => u.Username == login.Username).FirstOrDefault();

            if (user == null || !HelperMethods.VerifyPasswordHash(login.Password, user.Password))
                return null;

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Username),
                    new Claim(ClaimTypes.Role, user.Role)
                }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            return new AuthResponseDTO { Token = tokenString, Username = user.Username };
        }


        #endregion

        #region Update



        public User UpdateUser(int userId, string username, string password, string email)
        {
            var user =  _unitOfWork.Repository<User>().GetById(x=> x.UserId == userId).FirstOrDefault();

            if (user == null)
                throw new Exception("User not found");

            user.Username = username;
            user.Password = HelperMethods.HashPassword(password);

            _unitOfWork.Repository<User>().Update(user);
            _unitOfWork.Complete();

            return user;
        }


        #endregion

        #region Delete



        public Task DeleteUser(int userId)
        {
            var user = _unitOfWork.Repository<User>().GetById(x=> x.UserId == userId).FirstOrDefault();

            if (user == null)
                throw new Exception("User not found");

            _unitOfWork.Repository<User>().Delete(user);
            _unitOfWork.Complete();

            return Task.CompletedTask;
        }



        #endregion




    }
}
