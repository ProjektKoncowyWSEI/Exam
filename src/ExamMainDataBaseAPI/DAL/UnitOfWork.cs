using ExamContract.MainDbModels;
using ExamMainDataBaseAPI.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExamMainDataBaseAPI.DAL
{
    public class UnitOfWork
    {       
        public Repository<Question> QuestionRepo { get; private set; }
        public Repository<Answer> AnswersRepo { get; private set; }        

        public UnitOfWork(Repository<Question> questionRepo, Repository<Answer> answersRepo)
        {         
            this.QuestionRepo = questionRepo;
            this.AnswersRepo = answersRepo;          
        }

        public async Task<Question> GetQuestionWithAnswer(int id)
        {
            var question = await QuestionRepo.GetAsync(id); 
            question.Answers = await AnswersRepo.FindBy(a=>a.QuestionId == id).ToListAsync();
            return question;
        }   
    }
}
