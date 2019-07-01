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
using Microsoft.Extensions.Logging;
using ExamContract.MainDbModels;

namespace ExamMainDataBaseAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuestionsController : Controller 
    {
        private IUnitOfWork uow = null;     

        public QuestionsController(IUnitOfWork uow)
        {
            this.uow = uow;          
        }
        [HttpGet]
        public async Task<IEnumerable<Question>> GetAll()
        {            
            return await uow.QuestionRepo.GetAllAsync();
        }
       
        [HttpGet("{id}")]
        public async Task<Question> Get(int id)
        {
            return await uow.QuestionRepo.GetAsync(id);
        }
       
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, Question question)
        {
            if (id != question.Id)
            {
                return BadRequest();
            }
            await uow.QuestionRepo.UpdateAsync(question);
            await uow.SaveChangesAsync();
            return NoContent();
        }
        // POST: api/Question
        [HttpPost]
        public async Task<ActionResult<Question>> Post(Question question)
        {
            await uow.QuestionRepo.AddAsync(question);
            await uow.SaveChangesAsync();
            return CreatedAtAction("Get", new { id = question.Id }, question);
        }
        // DELETE: api/Question/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Question>> Delete(int id)
        {
            var question = await uow.QuestionRepo.GetAsync(id);
            if (question == null)
            {
                return NotFound();
            }
            await uow.QuestionRepo.RemoveAsync(question);
            await uow.SaveChangesAsync();
            return question;
        }
    }
}
