using ExamMainDataBaseAPI.DAL.Interface;
using ExamMainDataBaseAPI.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExamMainDataBaseAPI.DAL
{
    public class AnswersRepo : Repository<Answer>, IAnswersRep
    {
        public AnswersRepo(DbContext context) : base(context)
        {
        }
    }
}
