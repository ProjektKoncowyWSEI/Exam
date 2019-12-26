using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExamContract.MainDbModels;
using ExamMainDataBaseAPI.DAL;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ExamMainDataBaseAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuestionWithAnswersController : ControllerBase
    {
        private UnitOfWork uow = null;

        public QuestionWithAnswersController(UnitOfWork uow)
        {
            this.uow = uow;
        }
        [HttpGet("{id}")]
        public async Task<Question> Get(int id)
        {
            return await uow.GetQuestionWithAnswer(id); 
        }
    }
}