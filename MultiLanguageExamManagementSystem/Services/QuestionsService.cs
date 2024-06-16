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
}