using LifeEcommerce.Helpers;
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
        public AuthService(IConfiguration configuration, IUnitOfWork unitOfWork)
        {
            _configuration = configuration;
            _unitOfWork = unitOfWork;
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
    }
}
