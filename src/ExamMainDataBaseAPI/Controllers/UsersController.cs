using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ExamMainDataBaseAPI.DAL;
using System.Linq.Expressions;
using Microsoft.Extensions.Logging;
using ExamContract.MainDbModels;
using ExamContract.Auth;
using ExamContract.Auth;

namespace ExamMainDataBaseAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UsersController : MyBaseController<User>
    {
        private readonly Repository<User> repo;

        public UsersController(Repository<User> repo) : base(repo)
        {
            this.repo = repo;
        }
        [HttpGet("{examId}")] 
        [KeyAuthorize(RoleEnum.admin, RoleEnum.teacher, RoleEnum.student)]
        public virtual async Task<ActionResult<IEnumerable<User>>> GetUsersForExam(int examId)
        {
            return await repo.FindBy(a => a.ExamId == examId && a.Active == true).ToListAsync();
        }
    }
}