using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using ExamContract.TutorialModels;
using ExamTutorialsAPI.DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ExamTutorialAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TutorialsController : MyBaseController<Tutorial>
    {
        private readonly Repository<Tutorial> repo;       

        public TutorialsController(Repository<Tutorial> repo) : base(repo)
        {
            this.repo = repo;           
        }
        public override async Task<ActionResult<IEnumerable<Tutorial>>> Get(string login, bool? onlyActive = null)
        {
            List<Tutorial> list = new List<Tutorial>();
            Expression<Func<Tutorial, bool>> predicate;
            if (onlyActive == true && login == null)
                predicate = a => a.Active == true;
            else if (onlyActive != true && login != null)
                predicate = a => a.Login == login;
            else if (onlyActive == true && login != null)
                predicate = a => a.Active == true && a.Login == login;
            else
            {                
                list = await repo.GetListAsync();
                list.ForEach(a => a.Content = null);
                return list;
            }            
            list = await repo.FindBy(predicate).ToListAsync();
            list.ForEach(a => a.Content = null);
            return list;
        }
    }
}