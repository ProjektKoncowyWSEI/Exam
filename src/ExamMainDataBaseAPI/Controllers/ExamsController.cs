using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ExamContract.MainDbModels;
using ExamMainDataBaseAPI.Models;
using ExamMainDataBaseAPI.DAL;

namespace ExamMainDataBaseAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExamsController : MyBaseController<Exam>
    {
        public ExamsController(Repository<Exam> repo) : base(repo)
        {
        }
    }
}