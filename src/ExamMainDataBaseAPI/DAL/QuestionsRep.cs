using ExamMainDataBaseAPI.DAL.Interface;
using ExamMainDataBaseAPI.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExamMainDataBaseAPI.DAL
{
    public class QuestionsRep : Repository<Questions>, IQuestionsRep
    {
        public QuestionsRep(DbContext context) : base(context)
        {
        }
    }
}
