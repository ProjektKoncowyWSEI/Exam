using ExamContract.TutorialModels;
using ExamTutorialsAPI.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExamTutorialsAPI.DAL
{
    public class UnitOfWork
    {
        private  Repository<Tutorial> tutorialsRepo;

        public UnitOfWork(Context context, Repository<Tutorial> tutorialsRepo)
        {
            this.tutorialsRepo = tutorialsRepo;
        }
    }
}
