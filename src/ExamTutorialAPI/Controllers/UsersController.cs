using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExamContract.TutorialModels;
using ExamTutorialAPI.Controllers;
using ExamTutorialsAPI.DAL;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ExamTutorialAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : MyBaseController<User>
    {
        public UsersController(Repository<User> repo) : base(repo)
        {
        }
    }
}