using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExamMainDataBaseAPI.DAL.Interface;
using ExamMainDataBaseAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ExamMainDataBaseAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuestionWithAnswersController : ControllerBase
    {
        private IUnitOfWork uow = null;
        
        public QuestionWithAnswersController(IUnitOfWork uow)
        {
            this.uow = uow;
        }
        [HttpGet("{id}")]
        public async Task<Questions> Get(int id)
        {
            return await uow.GetQuestionWithAnswer(id);
        }
    }
}