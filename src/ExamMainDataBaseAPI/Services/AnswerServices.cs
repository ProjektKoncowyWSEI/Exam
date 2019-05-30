using ExamMainDataBaseAPI.Models;
using ExamMainDataBaseAPI.Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExamMainDataBaseAPI.Services
{
    public class AnswerServices : IAnswersServices
    {
        private ExamQuestionsDbContext context;

        public AnswerServices(ExamQuestionsDbContext context)
        {
            this.context = context;
        }

        public async Task AddAnswer(Answer answer)
        {
            context.Answer.Add(answer);
            await context.SaveChangesAsync();
        }

        public async Task DeleteAnswer(int id)
        {
            var answer = await context.Answer.FindAsync(id);
            if (answer != null)
            {
                context.Answer.Remove(answer);
                await context.SaveChangesAsync();
            }
        }

        public Task<Answer> GetAnswer(int id)
        {
            return context.Answer.FindAsync(id);
        }

        public List<Answer> GetAnswer()
        {
            return context.Answer.ToList();
        }

        public bool AnswerExists(int id)
        {
            return context.Answer.Any(e => e.Id == id);
        }

        public async Task UpdateAnswer(int id, Answer item)
        {
            var answer = await context.Answer.FindAsync(id);
            if (answer != null)
            {
                answer.Answer1 = item.Answer1;

                context.Entry(answer).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                await this.context.SaveChangesAsync();
            }
        }

        public List<Answer> GetAnswers()
        {
            return context.Answer.ToList();
        }
    }
}
