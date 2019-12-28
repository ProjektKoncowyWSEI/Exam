using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExamContract.Auth;
using ExamContract.CourseModels;
using ExamCourseAPI.DAL;
using ExamContract.Auth;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ExamCourseAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [KeyAuthorize(RoleEnum.admin)]
    public class KeysController : KeyApiController
    {
        public KeysController(ApiKeyRepo repo) : base(repo)
        {
        }
    }
}