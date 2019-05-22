using ExamMainDataBaseAPI.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExamMainDataBaseAPI.Services.Interface
{
   public interface IQuestionsServices
    {
        Task AddQuestion(Questions questions);

        Task DeleteQuestion(int id);

        Task<Questions> GetQuestion(int id);

        Task<List<Questions>> GetQuestions();

         bool QuestionExists(int id);

         Task UpdateQuestion(int id,Questions item);
    }
}

