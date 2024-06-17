using LifeEcommerce.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MultiLanguageExamManagementSystem.Data;
using MultiLanguageExamManagementSystem.Data.UnitOfWork;
using MultiLanguageExamManagementSystem.Models.Dtos;
using MultiLanguageExamManagementSystem.Models.Dtos.Exam;
using MultiLanguageExamManagementSystem.Models.Dtos.Question;
using MultiLanguageExamManagementSystem.Models.Entities;
using MultiLanguageExamManagementSystem.Services.IServices;

namespace MultiLanguageExamManagementSystem.Services;

public class ExamService : IExamService
{
    private readonly UnitOfWork _unitOfWork;

    public ExamService(UnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    #region Create


        public async Task<int> CreateExam(string title, DateTime startTime, DateTime endTime, int professorId)
        {

            var professor = _unitOfWork.Repository<User>().GetById(x => x.UserId == professorId).FirstOrDefault();

            if (professor == null || professor.Role != "Professor")
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


        public async Task<ExamResultDTO> SubmitExam(int userId, int examId, Dictionary<int, string> answers)
        {
            var exam = _unitOfWork.Repository<Exam>().GetByCondition(x => x.ExamId == examId).FirstOrDefault();

            if (exam == null)
                throw new Exception("Exam is null");


            var questions = _unitOfWork.Repository<ExamQuestion>()
                .GetByCondition(x => x.ExamId == examId)
                .Include(x => x.Question)
                .ToList();


            int correctAnswersCount = 0;
            foreach (var question in questions)
            {
                if (answers.ContainsKey(question.QuestionId) && (answers[question.QuestionId] == question.Question.CorrectAnswer))
                {
                    correctAnswersCount++;
                }
            }

            var score = (double)correctAnswersCount / answers.Count * 100;

            var examResult = new ExamResult
            {
                UserId = userId,
                ExamId = examId,
                TotalQuestions = questions.Count,
                CorrectAnswers = correctAnswersCount,
                Score = score,
            };

            _unitOfWork.Repository<ExamResult>().Create(examResult);
            _unitOfWork.Complete();

            return new ExamResultDTO
            {
                UserId = userId,
                ExamId = examId,
                TotalQuestions = questions.Count,
                CorrectAnswers = correctAnswersCount,
                Score = score
            };
        }
        #endregion

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
                PossibleAnswers = x.Question.possibleAnswers,
                CorrectAnswer = x.Question.CorrectAnswer,
                GivenAnswer = x.Question.GivenAnswer
            }).ToList()
        }).ToList();

        return examDetailsDTOs;
    }

    public Exam GetExam(int id)
    {
        return _unitOfWork.Repository<Exam>()
            .GetById(x => x.ExamId == id)
            .Include(e => e.ExamQuestions)
            .ThenInclude(eq => eq.Question)
            .FirstOrDefault();
    }

    public IEnumerable<RequestExam> GetExamRequests()
    {
        return _unitOfWork.Repository<RequestExam>()
            .GetByCondition(x => x.Status == RequestStatus.Pending);
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
                        Text = x.Question.QuestionText,
                        PossibleAsnwers = x.Question.possibleAnswers
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

    #region Update



    public async Task<Exam> UpdateExam(int examId, string title, DateTime startTime, DateTime endTime, int professorId)
    {
        var exam = _unitOfWork.Repository<Exam>().GetById(x => x.ExamId == examId).FirstOrDefault();

        if (exam == null)
            throw new Exception("Exam not found");

        exam.Title = title;
        exam.StartTime = startTime;
        exam.EndTime = endTime;
        exam.ProfessorId = professorId;

        _unitOfWork.Repository<Exam>().Update(exam);
        _unitOfWork.Complete();

        return exam;
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



    #endregion

    #region Delete



    public async Task DeleteExam(int examId)
    {
        var exam = _unitOfWork.Repository<Exam>().GetById(x => x.ExamId == examId).FirstOrDefault();

        if (exam == null)
            throw new Exception("Exam not found");

        _unitOfWork.Repository<Exam>().Delete(exam);
        _unitOfWork.Complete();
    }



    #endregion
}