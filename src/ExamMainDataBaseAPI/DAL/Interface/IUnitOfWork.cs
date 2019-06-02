using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExamMainDataBaseAPI.DAL.Interface
{
    public interface IUnitOfWork : IDisposable
    {
        IQuestionsRep Questions { get; }
        IAnswersRep Answers { get; }
        IQuestionAnswerRepo Qa { get; }
        int SaveChanges();
    }
}
