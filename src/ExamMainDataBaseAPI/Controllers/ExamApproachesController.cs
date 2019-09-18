using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using ExamContract;
using ExamContract.MainDbModels;
using ExamMainDataBaseAPI.DAL;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace ExamMainDataBaseAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ExamApproachesController : ControllerBase
    {
        private readonly ApproachesRepository repo;

        public ExamApproachesController(ApproachesRepository repo)
        {
            this.repo = repo;
        }
        [HttpPost]
        public async Task<ActionResult<ExamApproache>> Post(ExamApproache item)
        {
            await repo.AddAsync(item);
            await repo.SaveChangesAsync();
            return CreatedAtAction(nameof(Get), new { examId = item.ExamId, login = item.Login }, item);
        }
        [HttpPost]
        public async Task<ActionResult<ExamApproache>> PostResult(ExamApproacheResult item)
        {
            await repo.AddResultAsync(item);
            await repo.SaveChangesAsync();
            return CreatedAtAction(nameof(GetResult), new { examId = item.ExamId, login = item.Login, questionId = item.QuestionId, answerId = item.AnswerId }, item);
        }
        [HttpPost]
        public async Task<ActionResult<IEnumerable<ExamApproacheResult>>> PostResults(string model)
        {
            var items = JsonConvert.DeserializeObject<List<ExamApproacheResult>>(model);
            foreach (var item in items)
            {
                var x = repo.AddResultAsync(item).Result;
            }
            await repo.SaveChangesAsync();
            return CreatedAtAction(nameof(GetResults), new { examId = items.FirstOrDefault()?.ExamId, login = items.FirstOrDefault()?.Login }, items);
        }
        [HttpPut("{examId}")]
        public virtual async Task<IActionResult> Put(int examId, ExamApproache item)
        {
            if (examId != item.ExamId)
            {
                return BadRequest();
            }
            await repo.UpdateAsync(item);
            try
            {
                await repo.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!repo.FindBy(a => a.ExamId == examId).Any())
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return NoContent();
        }
        [HttpGet("{examId}/{login}")]
        public virtual async Task<ActionResult<ExamApproache>> Get(int examId, string login)
        {
            var item = await repo.GetAsync(examId, login);
            if (item == null)
                return NotFound();
            return item;
        }
        [HttpGet("{login}/{examId}/{questionId}/{answerId}")]
        public virtual async Task<ActionResult<ExamApproacheResult>> GetResult(string login, int examId, int questionId, int answerId)
        {
            var item = await repo.GetResultAsync(login, examId, questionId, answerId);
            if (item == null)
                return NotFound();
            return item;
        }
        [HttpGet]
        public virtual async Task<ActionResult<IEnumerable<ExamApproache>>> GetList(int? examId, string login)
        {
            Expression<Func<ExamApproache, bool>> predicate;
            if (examId != null && login == null)
                predicate = a => a.ExamId == examId;
            else if (examId == null && login != null)
                predicate = a => a.Login == login;
            else if (examId != null && login != null)
                predicate = a => a.ExamId == examId && a.Login == login;
            else
                return await repo.GetListAsync();
            return await repo.FindBy(predicate).ToListAsync();
        }
        [HttpGet]
        public virtual async Task<ActionResult<IEnumerable<ExamApproacheResult>>> GetResults(string login, int? examId)
        {
            return login == null && examId == null ? await repo.GetResultsListAsync() : await repo.GetResultsListAsync(login, examId);
        }
    }
}