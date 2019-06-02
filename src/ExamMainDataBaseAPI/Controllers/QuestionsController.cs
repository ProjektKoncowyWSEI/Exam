using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ExamMainDataBaseAPI.Models;
using ExamMainDataBaseAPI.DAL;
using ExamMainDataBaseAPI.DAL.Interface;
using System.Linq.Expressions;

namespace ExamMainDataBaseAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuestionsController : Controller 
    {
        private UnitOfWork uow = null;

        public QuestionsController(ExamQuestionsDbContext context)

        {
            this.uow = new UnitOfWork(context);
        }

        // GET: api/Questions
        [HttpGet]
        public Task <IEnumerable<Questions>> GetAll()
        {
            return uow.Questions.GetAll();
        }
        // GET: api/Questions/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Questions>> Get(int id)
        {
            return await uow.Questions.GetAsync(id);      
        }
        // PUT: api/Questions/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, Questions question)
        {
            if (id != question.Id)
            {
                return BadRequest();
            }
            await uow.Questions.UpdateAsync(question);
            uow.SaveChanges();
            return Content("PutSucces");
        }
        // POST: api/Questions
        [HttpPost]
        public async Task<ActionResult<Questions>> Post(Questions question)
        {
            await uow.Questions.Add(question);
            uow.SaveChanges();
            return CreatedAtAction("Get", new { id = question.Id }, question);
        }
        // DELETE: api/Questions/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var question = await uow.Questions.GetAsync(id);
            if (question == null)
            {
                return NotFound();
            }
            await uow.Questions.RemoveAsync(question);
            uow.SaveChanges();
            return Content("DeleteSucces");
        }
    }
}
