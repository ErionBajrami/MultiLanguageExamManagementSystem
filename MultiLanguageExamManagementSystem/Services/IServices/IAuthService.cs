using MultiLanguageExamManagementSystem.Models.Dtos;
using MultiLanguageExamManagementSystem.Models.Dtos.User;

namespace MultiLanguageExamManagementSystem.Services.IServices
{
    public interface IAuthService
    {
        AuthResponseDTO Authenticate(LoginDTO login);
        void SignUp(SignUpDTO signUp);
    }
}
