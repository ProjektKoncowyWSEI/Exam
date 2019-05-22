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
        private IQuestionsServices _questionsServices;

        public QuestionsController(IQuestionsServices questionsServices)
        {
            _questionsServices = questionsServices;
        }

        // GET: api/Questions
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Questions>>> GetQuestions()
        {
            return await _questionsServices.GetQuestions();
        }

        // GET: api/Questions/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Questions>> GetQuestion(int id)
        {
            var questions = await _questionsServices.GetQuestion(id);

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

            await _questionsServices.UpdateQuestion(id, questions);


            return NoContent();
        }

        // POST: api/Questions
        [HttpPost]
        public async Task<ActionResult> AddQuestions(Questions questions)
        {
            await _questionsServices.AddQuestion(questions);

            return CreatedAtAction("GetQuestions", new { id = questions.Id }, questions);
        }

        // DELETE: api/Questions/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteQuestions(int id)
        {
            await _questionsServices.DeleteQuestion(id);
            return Content("OK");
        }
    }
}
