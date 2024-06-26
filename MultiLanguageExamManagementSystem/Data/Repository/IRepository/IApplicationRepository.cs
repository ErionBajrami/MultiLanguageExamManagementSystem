﻿using MultiLanguageExamManagementSystem.Models.Entities;
using System.Linq.Expressions;

namespace MultiLanguageExamManagementSystem.Data.Repository.IRepository
{
    public interface IApplicationRepository<Tentity> where Tentity : class
    {
        IQueryable<Tentity> GetByCondition(Expression<Func<Tentity, bool>> expression);
        IQueryable<Tentity> GetById(Expression<Func<Tentity, bool>> expression);
        IQueryable<Tentity> GetAll();
        void Create(Tentity entity);
        void CreateRange(List<Tentity> entity);
        void Update(Tentity entity);
        void UpdateRange(List<Tentity> entity);
        void Delete(Tentity entity);
        void DeleteRange(List<Tentity> entity);
        Task SaveChangesAsync();



        #region Exam

        IEnumerable<RequestExam> GetUserRequests(int userId, int examId);

        #endregion
    }
}
