using ExamMainDataBaseAPI.Models;
using ExamMainDataBaseAPI.Services.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExamMainDataBaseAPI.Services
{
    public class QuestionsServices : IQuestionsServices
    {
        private ExamQuestionsDbContext context;

        public QuestionsServices(ExamQuestionsDbContext context)
        {
            this.context = context;
        }

        public async Task AddQuestion(Questions questions)
        {
             context.Questions.Add(questions);
        }

        public async Task DeleteQuestion(int id)
        {
            var questions = await context.Questions.FindAsync(id);
            if (questions != null)
            {
                context.Questions.Remove(questions);
            }
        }

        public Task<Questions> GetQuestion(int id)
        {
            return context.Questions.FindAsync(id);
        }

        public async Task<List<Questions>> GetQuestions()
        {
            return await context.Questions.ToListAsync();
        }

        public bool QuestionExists(int id)
        {
            return context.Questions.Any(e => e.Id == id);
        }

        public async Task UpdateQuestion(int id, Questions item)
        {
            var question = await context.Questions.FindAsync(id);
            if (question != null)
            {
                question.Question = item.Question;
                question.AnswerType = item.AnswerType;
                question.Image = item.Image;
                question.QuestionAnswer = item.QuestionAnswer;

                context.Entry(question).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                
            }
        }
    }
}
