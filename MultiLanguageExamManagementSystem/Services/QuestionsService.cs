using LifeEcommerce.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MultiLanguageExamManagementSystem.Data;
using MultiLanguageExamManagementSystem.Data.UnitOfWork;
using MultiLanguageExamManagementSystem.Models.Dtos.Question;
using MultiLanguageExamManagementSystem.Models.Entities;

namespace MultiLanguageExamManagementSystem.Services;

public class QuestionsService
{
    private readonly UnitOfWork _unitOfWork;

    public QuestionsService(UnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }


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
}