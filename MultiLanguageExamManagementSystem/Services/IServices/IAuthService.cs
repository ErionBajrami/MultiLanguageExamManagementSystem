using MultiLanguageExamManagementSystem.Models.Dtos;
using MultiLanguageExamManagementSystem.Models.Dtos.User;

namespace MultiLanguageExamManagementSystem.Services.IServices
{
    public interface IAuthService
    {
        
        

       
        

       


        #region Create



        void SignUp(SignUpDTO signUp);
        Task AddUser(User user);



        #endregion

        #region Read



        Task<List<User>> GetAllUsers();
        AuthResponseDTO Authenticate(LoginDTO login);



        #endregion

        #region Update

        User UpdateUser(int userId, string username, string password);

        #endregion

        #region Delete


        Task DeleteUser(int userId);


        #endregion

    }
}
