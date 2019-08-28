using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExamContract.TutorialModels;
using ExamTutorialsAPI.DAL;
using ExamTutorialsAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace ExamTutorialAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TutorialsController : MyBaseController<Tutorial>
    {
        public TutorialsController(Repository<Tutorial> repo) : base(repo)
        {
        }
    }
}