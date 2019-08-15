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
        private  Repository<Category> categoriesRepo;

        public UnitOfWork(ExamTutorialsApiContext context, Repository<Tutorial> tutorialsRepo, Repository<Category> categoriesRepo)
        {
            this.tutorialsRepo = tutorialsRepo;
            this.categoriesRepo = categoriesRepo;
        }
    }
}
