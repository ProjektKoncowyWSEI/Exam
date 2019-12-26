using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExamContract.MainDbModels;
using ExamMainDataBaseAPI.DAL;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ExamMainDataBaseAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AnswersController : MyBaseController<Answer>
    {
        public AnswersController(Repository<Answer> repo) : base(repo)
        {
        }
    }
}