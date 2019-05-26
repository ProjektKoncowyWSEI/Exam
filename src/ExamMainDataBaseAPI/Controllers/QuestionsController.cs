using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ExamMainDataBaseAPI.Models;
using ExamMainDataBaseAPI.Services;
using ExamMainDataBaseAPI.Services.Interface;

namespace ExamMainDataBaseAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuestionsController : Controller
    {
        private Repository_Unit_of_Work uow = null;

        public QuestionsController(DbContextOptions<ExamQuestionsDbContext> options)
        {
            uow = new Repository_Unit_of_Work(options);
        }
     
        // GET: api/Questions
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Questions>>> GetQuestions()
        {
            return await uow.QuestionsRepository.GetQuestions();
        }

        // GET: api/Questions/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Questions>> GetQuestion(int id)
        {
            var questions = await uow.QuestionsRepository.GetQuestion(id);
            if (questions == null)
            {
                return NotFound();
            }
            return questions;
        }

        // PUT: api/Questions/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateQuestion(int id, Questions questions)
        {
            if (id == questions.Id)
            {
                return BadRequest();
            }

            await uow.QuestionsRepository.UpdateQuestion(id, questions);
            uow.SaveChanges();

            return Content("PutSucces");
        }

        // POST: api/Questions
        [HttpPost]
        public async Task<ActionResult<Questions>> AddQuestions(Questions questions)
        {
            await uow.QuestionsRepository.AddQuestion(questions);
            uow.SaveChanges();
            return CreatedAtAction("GetQuestions", new { id = questions.Id }, questions);
        }

        // DELETE: api/Questions/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteQuestions(int id)
        {
            await uow.QuestionsRepository.DeleteQuestion(id);
            uow.SaveChanges();
            return Content("DeleteSucces");
        }
    }
}
