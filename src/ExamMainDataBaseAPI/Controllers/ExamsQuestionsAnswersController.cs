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
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ExamsQuestionsAnswersController : ControllerBase
    {
        private UnitOfWork uow = null;

        public ExamsQuestionsAnswersController(UnitOfWork uow)
        {
            this.uow = uow;
        }
        [HttpGet("{id}")]
        public async Task<Exam> GetById(int id)
        {
            return await uow.GetExamwithQuestionsWithAnswers(id);
        }
        [HttpGet("{code}")]
        public async Task<Exam> GetByCode(string code)
        {
            return await uow.GetExamwithQuestionsWithAnswers(code);
        }
        [HttpPost]
        public async Task<ActionResult> Post(Exam item)
        {           
            await uow.Clone(item);
            return CreatedAtAction(nameof(GetById), new { id = item.Id }, item);
        }
    }
}