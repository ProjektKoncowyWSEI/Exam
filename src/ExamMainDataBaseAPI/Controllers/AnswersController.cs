using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExamMainDataBaseAPI.DAL;
using ExamMainDataBaseAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ExamMainDataBaseAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AnswersController : ControllerBase
    {
        private UnitOfWork uow = null;

        public AnswersController(ExamQuestionsDbContext context)

        {
            this.uow = new UnitOfWork(context);
        }

        // GET: api/Answer
        [HttpGet]
        public Task<IEnumerable<Answer>> GetAll()
        {
            return uow.Answers.GetAll();
        }
        // GET: api/Answer/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Answer>> Get(int id)
        {
            return await uow.Answers.GetAsync(id);
        }
        // PUT: api/Answer/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, Answer answer)
        {
            if (id != answer.Id)
            {
                return BadRequest();
            }
            await uow.Answers.UpdateAsync(answer);
            uow.SaveChanges();
            return Content("PutSucces");
        }
        // POST: api/Answer
        [HttpPost]
        public async Task<ActionResult<Answer>> Post(Answer answer)
        {
            await uow.Answers.Add(answer);
            uow.SaveChanges();
            return CreatedAtAction("Get", new { id = answer.Id }, answer);
        }
        // DELETE: api/Answer/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var answer = await uow.Answers.GetAsync(id);
            if (answer == null)
            {
                return NotFound();
            }
            await uow.Answers.RemoveAsync(answer);
            uow.SaveChanges();
            return Content("DeleteSucces");
        }
    }
}