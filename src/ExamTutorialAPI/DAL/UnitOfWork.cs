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
        private readonly Context context;
        private  Repository<Tutorial> tutorialsRepo;

        public UnitOfWork(Context context, Repository<Tutorial> tutorialsRepo)
        {
            this.context = context;
            this.tutorialsRepo = tutorialsRepo;
        }
        public async Task<List<Tutorial>> GetTutorialListAsyncNoContent()
        {
            var list = await tutorialsRepo.GetListAsync();
            list.ForEach(a => a.Content = null);
            return list;
        }
    }
}
