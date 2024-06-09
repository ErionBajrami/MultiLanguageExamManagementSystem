using MultiLanguageExamManagementSystem.Models.Entities;

public class User
{
    public int UserId { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    public UserRoleEnum RoleEnum { get; set; } 
}
