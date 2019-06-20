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
using Logger;
using Microsoft.Extensions.Logging;
using Logger.Data;

namespace ExamMainDataBaseAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuestionsController : Controller 
    {
        private IUnitOfWork uow = null;
        private readonly ILogger logger;

        public QuestionsController(IUnitOfWork uow,ILogger logger)
        {
            this.uow = uow;
            this.logger = logger;
        }
        [HttpGet]
        public async Task<IEnumerable<Questions>> GetAll()
        {
            //logger.LogInformation("cos");
            return await uow.QuestionRepo.GetAllAsync();
        }
       
        [HttpGet("{id}")]
        public async Task<Questions> Get(int id)
        {
            return await uow.QuestionRepo.GetAsync(id);
        }
       
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, Questions question)
        {
            if (id != question.Id)
            {
                return BadRequest();
            }
            await uow.QuestionRepo.UpdateAsync(question);
            await uow.SaveChangesAsync();
            return NoContent();
        }
        // POST: api/Questions
        [HttpPost]
        public async Task<ActionResult<Questions>> Post(Questions question)
        {
            await uow.QuestionRepo.AddAsync(question);
            await uow.SaveChangesAsync();
            return CreatedAtAction("Get", new { id = question.Id }, question);
        }
        // DELETE: api/Questions/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Questions>> Delete(int id)
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
