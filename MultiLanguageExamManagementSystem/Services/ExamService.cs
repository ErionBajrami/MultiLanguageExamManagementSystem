using LifeEcommerce.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MultiLanguageExamManagementSystem.Data;
using MultiLanguageExamManagementSystem.Data.UnitOfWork;
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


    public IEnumerable<Exam> GetAvailableExams()
    {
        return _unitOfWork.Repository<Exam>().GetAll();
    }


    public async Task<int> CreateExam(string title, DateTime startTime, DateTime endTime, int professorId)
    {
        // Create a new exam instance
        var exam = new Exam
        {
            Title = title,
            StartTime = startTime,
            EndTime = endTime,
            ProfessorId = professorId
        };

        var allQuestions = await _unitOfWork.Repository<Question>().GetAll().ToListAsync();

   
        HelperMethods.Shuffle(allQuestions);
        var selectedQuestions = allQuestions.Take(10).ToList();

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

    public async Task<IEnumerable<QuestionRetrieveDTO>> GetAllQuestions()
    {
        var questions = await _unitOfWork.Repository<Question>()
            .GetAll()
            .Select(q => new QuestionRetrieveDTO
            {
                Text = q.QuestionText
            })
            .ToListAsync();

        return questions;
    }

    public async Task<string> AddQuestions(string questionText, string correctAnswer)
    {
        var newQuestion = new Question
        {
            QuestionText = questionText,
            CorrectAnswer = correctAnswer
        };

        _unitOfWork.Repository<Question>().Create(newQuestion);
        _unitOfWork.Complete();

        return newQuestion.QuestionText; 
    }



    public async Task RequestExam(int userId, int examId)
    {
        var existingRequests = _unitOfWork.Repository<RequestExam>().GetUserRequests(userId, examId);
        if (existingRequests.Count() >= 3)
            throw new InvalidOperationException("No more attempts left");

        var reqExam = new RequestExam
        {
            UserId = userId,
            ExamId = examId,
            Status = RequestStatus.Pending,
            RequestDate = DateTime.Now
        };
        
        _unitOfWork.Repository<RequestExam>().Create(reqExam);
        _unitOfWork.Complete();
    }


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

    public IEnumerable<Exam> GetApprovedExams(int userId)
    {
        var approvedExams = _unitOfWork.Repository<RequestExam>().GetByCondition(x => x.UserId == userId && x.Status == RequestStatus.Approved);

        return approvedExams.Select(x => x.Exam);
    }


    public Exam GetExamQuestions(int examId)
    {
        var exam = _unitOfWork.Repository<Exam>()
            .GetByCondition(e => e.ExamId == examId)
            .Include(e => e.ExamQuestions) 
            .FirstOrDefault();

        return exam;
    }



}