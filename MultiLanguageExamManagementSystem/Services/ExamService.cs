using LifeEcommerce.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MultiLanguageExamManagementSystem.Data;
using MultiLanguageExamManagementSystem.Data.UnitOfWork;
using MultiLanguageExamManagementSystem.Models.Dtos;
using MultiLanguageExamManagementSystem.Models.Dtos.Question;
using MultiLanguageExamManagementSystem.Models.Entities;

namespace MultiLanguageExamManagementSystem.Services;

public class ExamService
{
    private readonly UnitOfWork _unitOfWork;

    public ExamService(UnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }


    #region Read



    public IEnumerable<Exam> GetAvailableExams()
    {
        return _unitOfWork.Repository<Exam>().GetAll();
    }


    public async Task<List<ExamDetailsDTO>> GetAllExamDetailsAsync()
    {
        var exams = await _unitOfWork.Repository<Exam>()
            .GetAll()
            .Include(x => x.Professor)
            .Include(x => x.ExamQuestions)
            .ThenInclude(x => x.Question)
            .ToListAsync();

        var examDetailsDTOs = exams.Select(exam => new ExamDetailsDTO
        {
            ExamId = exam.ExamId,
            Title = exam.Title,
            StartTime = exam.StartTime,
            EndTime = exam.EndTime,
            ProfessorId = exam.ProfessorId,
            ProfessorName = exam.Professor?.Username,
            ApprovedRequest = exam.ApprovedRequest,
            ExamQuestions = exam.ExamQuestions.Select(x => new QuestionDetailsDTO
            {
                Text = x.Question.QuestionText,
                CorrectAnswer = x.Question.CorrectAnswer,
                GivenAnswer = x.Question.GivenAnswer
            }).ToList()
        }).ToList();

        return examDetailsDTOs;
    }


    public IEnumerable<RequestExam> GetExamRequests()
    {
        return _unitOfWork.Repository<RequestExam>()
            .GetByCondition(x => x.Status == RequestStatus.Rejected);
    }

    public ExamQuestionsDTO GetExamQuestions(int examId)
    {
        var exam = _unitOfWork.Repository<Exam>()
              .GetByCondition(x => x.ExamId == examId)
              .Include(x => x.ExamQuestions)
                  .ThenInclude(x => x.Question)
              .FirstOrDefault();

        if (exam == null)
            throw new Exception("Exam is null"); // Return an empty collection if exam is not found

        var examQuestionsDTO = new ExamQuestionsDTO
        {
            Title = exam.Title,
            ExamQuestions = exam.ExamQuestions
                    .Select(x => new QuestionRetrieveDTO
                    {
                        Text = x.Question.QuestionText
                    })
                    .ToList()
        };

        return examQuestionsDTO;
    }


    public IEnumerable<Exam> GetApprovedExams(int userId)
    {
        var approvedExams = _unitOfWork.Repository<RequestExam>().GetByCondition(x => x.UserId == userId && x.Status == RequestStatus.Approved);

        return approvedExams.Select(x => x.Exam);
    }

    #endregion

    #region Create


    public async Task<int> CreateExam(string title, DateTime startTime, DateTime endTime, int professorId)
    {

        var professor = _unitOfWork.Repository<User>().GetById(x => x.UserId == professorId).FirstOrDefault();

        if (professor == null || professor.RoleEnum != UserRoleEnum.Professor)
            throw new Exception("Professor is null or User is not a Professor");


        var allQuestions = await _unitOfWork.Repository<Question>().GetAll().ToListAsync();

        HelperMethods.Shuffle(allQuestions);

        var selectedQuestions = allQuestions.Take(10).ToList();



        var exam = new Exam
        {
            Title = title,
            StartTime = startTime,
            EndTime = endTime,
            ProfessorId = professorId,
            Professor = professor
        };

        foreach (var question in selectedQuestions)
        {
            var examQuestion = new ExamQuestion
            {
                Exam = exam,
                Question = question
            };

            exam.ExamQuestions.Add(examQuestion);
        }
        _unitOfWork.Repository<Exam>().Create(exam);
        _unitOfWork.Complete();

        return exam.ExamId;
    }

    public async Task RequestExam(int userId, int examId)
    {
        var existingRequests = _unitOfWork.Repository<RequestExam>().GetUserRequests(userId, examId);
        if (existingRequests.Count() >= 3)
            throw new Exception("No more attempts left");

        var reqExam = new RequestExam
        {
            UserId = userId,
            ExamId = examId,
            Status = RequestStatus.Pending,
            RequestDate = DateTime.UtcNow
        };

        _unitOfWork.Repository<RequestExam>().Create(reqExam);
        _unitOfWork.Complete();
    }



    #endregion

    #region Update
    public bool ApproveRequest(int requestId)
    {
        var request =  _unitOfWork.Repository<RequestExam>().GetById(x => x.RequestExamId == requestId).FirstOrDefault();

        if(request != null && request.Status == RequestStatus.Pending)
        {
            request.Status = RequestStatus.Approved;
            _unitOfWork.Repository<RequestExam>().Update(request);

            _unitOfWork.Complete();
        }

        return false;
    }


    public bool RejectRequest(int requestId)
    {
        var request = _unitOfWork.Repository<RequestExam>().GetById(x => x.RequestExamId == requestId).FirstOrDefault();

        if(request != null && request.Status == RequestStatus.Pending)
        {
            request.Status = RequestStatus.Rejected;
            _unitOfWork.Repository<RequestExam>().Update(request);

            _unitOfWork.Complete();
        }

        return false;
    }

    #endregion

}