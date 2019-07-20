using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using ExamContract.CourseModels;
using ExamCourseAPI.DAL;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ExamCourseAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TutorialsController : MyBaseTwoKeyController<TutorialCourse>
    {
        public TutorialsController(TwoKeysRepository<TutorialCourse> repo) : base(repo)
        {

        }
    }
}