using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ExamMainDataBaseAPI.Models;

namespace ExamMainDataBaseAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AnswersController : ControllerBase
    {
        private Repository_Unit_of_Work uow = null;

        public AnswersController(DbContextOptions<ExamQuestionsDbContext> options)
        {
            uow = new Repository_Unit_of_Work(options);
        }

        //// GET: api/Answers
        //[HttpGet]
        //public List<Answer> GetAnswer()
        //{
        //    return  uow.AnswersServices.GetAnswers().ToList();
        //}

        //// GET: api/Answers/5
        //[HttpGet("{id}")]
        //public async Task<ActionResult<Answer>> GetAnswer(int id)
        //{
        //    var answer = await uow.AnswersServices.FindAsync(id);

        //    if (answer == null)
        //    {
        //        return NotFound();
        //    }

        //    return answer;
        //}

        //// PUT: api/Answers/5
        //[HttpPut("{id}")]
        //public async Task<IActionResult> PutAnswer(int id, Answer answer)
        //{
        //    if (id != answer.Id)
        //    {
        //        return BadRequest();
        //    }

        //    _context.Entry(answer).State = EntityState.Modified;

        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!AnswerExists(id))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return NoContent();
        //}

        //// POST: api/Answers
        //[HttpPost]
        //public async Task<ActionResult<Answer>> PostAnswer(Answer answer)
        //{
        //    _context.Answer.Add(answer);
        //    await _context.SaveChangesAsync();

        //    return CreatedAtAction("GetAnswer", new { id = answer.Id }, answer);
        //}

        //// DELETE: api/Answers/5
        //[HttpDelete("{id}")]
        //public async Task<ActionResult<Answer>> DeleteAnswer(int id)
        //{
        //    var answer = await _context.Answer.FindAsync(id);
        //    if (answer == null)
        //    {
        //        return NotFound();
        //    }

        //    _context.Answer.Remove(answer);
        //    await _context.SaveChangesAsync();

        //    return answer;
        //}

        //private bool AnswerExists(int id)
        //{
        //    return _context.Answer.Any(e => e.Id == id);
        //}
    }
}
