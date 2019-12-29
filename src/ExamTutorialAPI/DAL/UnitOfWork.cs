using ExamContract.TutorialModels;
using System.Collections.Generic;
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
