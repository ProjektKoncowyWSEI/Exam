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
using ExamMainDataBaseAPI.DAL;
using ExamMainDataBaseAPI.DAL.Interface;
using System.Linq.Expressions;

namespace ExamMainDataBaseAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuestionsController : Controller /*, IRepository<Questions>*/
    {
        private UnitOfWork uow = null;

        public QuestionsController(ExamQuestionsDbContext context)

        {
            this.uow = new UnitOfWork(context);
        }


        // GET: api/Questions
        [HttpGet]
        public IEnumerable<Questions> GetAll()
        {
            return uow.Questions.GetAll();
        }

        // GET: api/Questions/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Questions>> Get(int id)
        {
            var questions =  uow.Questions.Get(id);
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

            var result =  uow.Questions.Get(id);
            
            

            return Content("PutSucces");
        }

        //// POST: api/Questions
        //[HttpPost]
        //public async Task<ActionResult<Questions>> AddQuestions(Questions questions)
        //{
        //    await uow.QuestionsRepository.AddQuestion(questions);
        //    uow.SaveChanges();
        //    return CreatedAtAction("GetQuestions", new { id = questions.Id }, questions);
        //}

        //// DELETE: api/Questions/5
        //[HttpDelete("{id}")]
        //public async Task<ActionResult> DeleteQuestions(int id)
        //{
        //    await uow.QuestionsRepository.DeleteQuestion(id);
        //    uow.SaveChanges();
        //    return Content("DeleteSucces");
        //}

       

        //public IEnumerable<Questions> Find(Expression<Func<Questions, bool>> predicate)
        //{
        //    throw new NotImplementedException();
        //}

        //public void Add(Questions entity)
        //{
        //    throw new NotImplementedException();
        //}

        //public void AddRange(IEnumerable<Questions> entities)
        //{
        //    throw new NotImplementedException();
        //}

        //public void Remove(Questions entity)
        //{
        //    throw new NotImplementedException();
        //}

        //public void RemoveRange(IEnumerable<Questions> entities)
        //{
        //    throw new NotImplementedException();
        //}
    }
}
