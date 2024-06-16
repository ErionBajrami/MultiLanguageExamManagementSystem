using MultiLanguageExamManagementSystem.Models.Entities;

namespace MultiLanguageExamManagementSystem.Models.Dtos.User
{
    public class UserRetrieveDTO
    {
        public string Username { get; set; }
        public UserRoleEnum RoleEnum { get; set; }
    }
}
