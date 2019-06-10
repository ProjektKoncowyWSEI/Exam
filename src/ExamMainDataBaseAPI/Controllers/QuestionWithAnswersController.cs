using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExamMainDataBaseAPI.DAL.Interface;
using ExamMainDataBaseAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ExamMainDataBaseAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuestionWithAnswersController : ControllerBase
    {
        private IUnitOfWork uow = null;
        private readonly ILogger _logger;

        public QuestionWithAnswersController(IUnitOfWork uow,ILogger logger)
        {
            this.uow = uow;
            _logger = logger;
        }
        [HttpGet("{id}")]
        public async Task<Questions> Get(int id)
        {
            _logger.LogError("ERROR");
            return await uow.GetQuestionWithAnswer(id); 
        }
    }
}