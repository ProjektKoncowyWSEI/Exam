using ExamMainDataBaseAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExamMainDataBaseAPI.DAL.Interface
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<Questions> QuestionRepo { get; }
        IRepository<Answer> AnswersRepo { get; }
        IRepository<QuestionAnswer> QuestionAnswerRepo { get; }
        Task SaveChangesAsync();
    }
}
