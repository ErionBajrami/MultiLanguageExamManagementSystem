using Microsoft.EntityFrameworkCore;
using MultiLanguageExamManagementSystem.Data;
using MultiLanguageExamManagementSystem.Data.UnitOfWork;
using MultiLanguageExamManagementSystem.Models.Entities;

namespace MultiLanguageExamManagementSystem.Services;

public class ExamService
{
    private readonly UnitOfWork _unitOfWork;
    private readonly ApplicationDbContext _context;

    public ExamService(UnitOfWork unitOfWork, ApplicationDbContext context)
    {
        _unitOfWork = unitOfWork;
        _context = context;
    }
    
    
    public async Task RequestExam(int userId, int examId)
    {
        var user = await _unitOfWork.Repository<User>().GetById(x => x.UserId == userId).FirstOrDefaultAsync();
        var exam = await _unitOfWork.Repository<Exam>().GetById(x => x.ExamId == examId).FirstOrDefaultAsync();

        if (user == null)
            return;
        
        if(exam == null)
            return;


        var countRequests = _unitOfWork.Repository<RequestExam>()
            .GetById(x => x.Exam.ExamId == examId && x.Student.UserId == userId)
            .ToList();

        if (countRequests.Count() > 3)
            return;

        var reqExam = new RequestExam
        {
            Student = user,
            Exam = exam
        };
        
        _unitOfWork.Repository<RequestExam>().Create(reqExam);
        _unitOfWork.Complete();
    }

    // public async Task ApproveRequest(int adminId, int userId, int examId) // prof id, user id, exam id
    // {
    //     var userProfessor = _unitOfWork.Repository<User>()
    //         .GetById(x => x.UserId == adminId)
    //         .Where(x => x.RoleEnum == UserRoleEnum.Admin);
    //     
    //     var User = _unitOfWork.Repository<User>()
    //         .GetById(x => x.UserId == userId)
    //         .Where(x => x.RoleEnum == UserRoleEnum.Student);
    //     
    //     var examToBeApproved = _unitOfWork.Repository<RequestExam>()
    //         .GetById(x => x.ExamId == examId && x.StudentId == userId);
    //
    //     if (userProfessor == null)
    //         return;
    //     
    //     if(examToBeApproved == null)
    //         return;
    //
    //     examToBeApproved.Exam.ApprovedRequest = true;
    //     
    //     _unitOfWork.Complete();
    // }
    
    
    public async Task ApproveRequest(int adminId, int userId, int examId)
    {
        // Check if the admin exists
        var admin = _unitOfWork.Repository<User>()
            .GetById(x => x.UserId == adminId && x.RoleEnum == UserRoleEnum.Admin);

        if (admin == null)
            throw new UnauthorizedAccessException("Invalid Admin");

        // Check if the student exists
        var student = _unitOfWork.Repository<User>()
            .GetById(x => x.UserId == userId && x.RoleEnum == UserRoleEnum.Student);

        if (student == null)
            throw new ArgumentException("Invalid Student");

        // Check if the exam request exists
        // var examRequest = _unitOfWork.Repository<RequestExam>()
        //     .GetByCondition(x => x.ExamId == examId && x.StudentId == userId);
        
        var examRequest = _context.Exam.

        if (examRequest == null)
            throw new ArgumentException("Invalid Exam Request");

        // Approve the exam request
        examRequest.Exam.ApprovedRequest = true;

        // Save changes
        await _unitOfWork.SaveAsync();
    }

}