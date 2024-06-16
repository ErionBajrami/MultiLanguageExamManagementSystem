using MultiLanguageExamManagementSystem.Models.Entities;

namespace MultiLanguageExamManagementSystem.Models.Dtos.User
{
    public class AddUserDTO
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public UserRoleEnum RoleEnum { get; set; }
    }
}
