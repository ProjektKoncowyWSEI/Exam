using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExamContract.MainDbModels;
using ExamMainDataBaseAPI.DAL;
using ExamMainDataBaseAPI.DAL.Interface;
using ExamMainDataBaseAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ExamMainDataBaseAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AnswersController : ControllerBase
    {
        private IUnitOfWork uow = null;
        public AnswersController(IUnitOfWork uow)
        {
            this.uow = uow;
        }

        [HttpGet]
        public async Task<IEnumerable<Answer>> GetAll()
        {
            return await uow.AnswersRepo.GetAllAsync();
        }
      
        [HttpGet("{id}")]
        public async Task<ActionResult<Answer>> Get(int id)
        {
            return await uow.AnswersRepo.GetAsync(id);
        }
       
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, Answer answer)
        {
            if (id != answer.Id)
            {
                return BadRequest();
            }
            await uow.AnswersRepo.UpdateAsync(answer);
            await uow.SaveChangesAsync();
            return NoContent();
        }
       
        [HttpPost]
        public async Task<ActionResult<Answer>> Post(Answer answer)
        {
            await uow.AnswersRepo.AddAsync(answer);
            await uow.SaveChangesAsync();
            return CreatedAtAction(nameof(Get), new { id = answer.Id }, answer);
        }
       
        [HttpDelete("{id}")]
        public async Task<ActionResult<Answer>> Delete(int id)
        {
            var answer = await uow.AnswersRepo.GetAsync(id);
            if (answer == null)
            {
                return NotFound();
            }
            await uow.AnswersRepo.RemoveAsync(answer);
            await uow.SaveChangesAsync();
            return answer;
        }
    }
}