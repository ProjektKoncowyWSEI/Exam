using ExamMainDataBaseAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExamMainDataBaseAPI.Services.Interface
{
   public interface IAnswersServices
    {
       
            Task AddAnswer(Answer answers);

            Task DeleteAnswer(int id);

            Task<Answer> GetAnswer(int id);

            List<Answer> GetAnswers();

            bool AnswerExists(int id);

            Task UpdateAnswer(int id, Answer item);
        }
 }

