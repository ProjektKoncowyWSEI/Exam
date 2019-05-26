using ExamMainDataBaseAPI.Models;
using ExamMainDataBaseAPI.Services;
using ExamMainDataBaseAPI.Services.Interface;
using Microsoft.EntityFrameworkCore;
using System;

namespace ExamMainDataBaseAPI.Controllers
{
    internal class Repository_Unit_of_Work : IDisposable
    {
        private ExamQuestionsDbContext db = null;
        public Repository_Unit_of_Work(DbContextOptions<ExamQuestionsDbContext> options)
        {
            this.db = new ExamQuestionsDbContext(options);
        }
        // Obsługę każdego repozytorium dodajemy tutaj
        IQuestionsServices questionRepository = null;
        IAnswersServices answersRepository = null;
        // Gettery dla każdego repozytorium dodajemy tutaj
        public IQuestionsServices QuestionsRepository
        {
            get
            {
                if (questionRepository == null)
                    questionRepository = new QuestionsServices(db);
                return questionRepository;
            }
        }

        public IAnswersServices AnswersServices
        {
            get
            {
                if (answersRepository == null)
                    answersRepository = new AnswerServices(db);
                return answersRepository;
            }
        }
        public void SaveChanges()
        {
            db.SaveChanges();
        }
        private bool disposed = false;
        public void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                    db.Dispose();
            }
            disposed = true;
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
