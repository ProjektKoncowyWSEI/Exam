using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExamContract.MainDbModels;
using ExamMainDataBaseAPI.DAL;
using ExamMainDataBaseAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ExamMainDataBaseAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExamsQuestionsAnswersController : ControllerBase
    {
        private UnitOfWork uow = null;

        public ExamsQuestionsAnswersController(UnitOfWork uow)
        {
            this.uow = uow;
        }
        [HttpGet("{id}")]
        public async Task<Exam> Get(int id)
        {
            return await uow.GetExamwithQuestionsWithAnswers(id);
        }
        [HttpPost]
        public async Task<ActionResult> Post(Exam item)
        {           
            await uow.Clone(item);
            return CreatedAtAction(nameof(Get), new { id = item.Id }, item);
        }
    }
}