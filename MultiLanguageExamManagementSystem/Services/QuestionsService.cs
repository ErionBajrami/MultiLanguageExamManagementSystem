using LifeEcommerce.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MultiLanguageExamManagementSystem.Data;
using MultiLanguageExamManagementSystem.Data.UnitOfWork;
using MultiLanguageExamManagementSystem.Models.Dtos.Question;
using MultiLanguageExamManagementSystem.Models.Entities;
using MultiLanguageExamManagementSystem.Services.IServices;

namespace MultiLanguageExamManagementSystem.Services;

public class QuestionsService : IQuestionService
{
    private readonly UnitOfWork _unitOfWork;

    public QuestionsService(UnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    #region Create


    public async Task<Question> AddQuestions(string questionText, string possibleAnswers, string correctAnswer)
    {
        var newQuestion = new Question
        {
            QuestionText = questionText,
            possibleAnswers = possibleAnswers,
            CorrectAnswer = correctAnswer
        };
        _unitOfWork.Repository<Question>().Create(newQuestion);
        _unitOfWork.Complete();

        return newQuestion;
    }



    #endregion

    #region Read



    public async Task<IEnumerable<QuestionRetrieveDTO>> GetAllQuestionsUser()
    {
        var questions = await _unitOfWork.Repository<Question>()
            .GetAll()
            .Select(x => new QuestionRetrieveDTO
            {
                Text = x.QuestionText,
                PossibleAsnwers = x.possibleAnswers
            })
            .ToListAsync();

        return questions;
    }

    public async Task<IEnumerable<QuestionInsertDTO>> GetAllQuestionsProfessor()
    {
        var questions = await _unitOfWork.Repository<Question>()
            .GetAll()
            .Select(x => new QuestionInsertDTO
            {
                Text = x.QuestionText,
                PossibleAnswers = x.possibleAnswers,
                CorrectAnswer = x.CorrectAnswer
            })
            .ToListAsync();

        return questions;
    }



    #endregion

    #region Update


    public async Task<Question> UpdateQuestion(int questionId, string questionText, string possibleAnswers, string correctAnswer)
    {
        var question = _unitOfWork.Repository<Question>().GetById(x => x.QuestionId == questionId).FirstOrDefault();

        if (question == null)
            throw new Exception("Question not found");

        question.QuestionText = questionText;
        question.possibleAnswers = possibleAnswers;
        question.CorrectAnswer = correctAnswer;

        _unitOfWork.Repository<Question>().Update(question);
        _unitOfWork.Complete();

        return question;
    }



    #endregion

    #region Delete


    public async Task DeleteQuestion(int questionId)
    {
        var question = _unitOfWork.Repository<Question>().GetById(x => x.QuestionId == questionId).FirstOrDefault();

        if (question == null)
            throw new Exception("Question not found");

        _unitOfWork.Repository<Question>().Delete(question);
        _unitOfWork.Complete();
    }



    #endregion
}