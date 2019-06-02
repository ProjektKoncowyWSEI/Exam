using ExamMainDataBaseAPI.DAL.Interface;
using ExamMainDataBaseAPI.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExamMainDataBaseAPI.DAL
{
    public class UnitOfWork : IUnitOfWork
    {

        private readonly ExamQuestionsDbContext context;
        public IQuestionsRep Questions { get; private set; }
        public IAnswersRep Answers { get; private set; }

        public UnitOfWork(ExamQuestionsDbContext context) {
            this.context = context;
            Questions = new QuestionsRep(context);
            Answers = new AnswersRepo(context);
        }

        public int SaveChanges()
        {
           return context.SaveChanges();
        }     
        public void Dispose()
        {
            context.Dispose();
        }
    }
}
