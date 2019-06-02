using ExamMainDataBaseAPI.DAL.Interface;
using ExamMainDataBaseAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace ExamMainDataBaseAPI.DAL
{
    internal class QuestionAnswerRepo : Repository<QuestionAnswer>, IQuestionAnswerRepo
    {
        public QuestionAnswerRepo(DbContext context) : base(context)
        {
        }
    }
}