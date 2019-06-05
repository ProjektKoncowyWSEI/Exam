using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExamContract;
using ExamMailSenderAPI.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ExamMailSenderAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MyBaseController<T> : ControllerBase where T : Entity
    {
        private readonly Repository<T> repo;

        public MyBaseController(Repository<T> repository)
        {
            repo = repository;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<T>>> Get()
        {
            return await repo.GetListAsync();
        }

        // GET: api/Mails/5
        [HttpGet("{id}")]
        public async Task<ActionResult<T>> Get(int id)
        {
            var item = await repo.GetAsync(id);
            if (item == null)
                return NotFound();
            return item;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, T item)
        {
            if (id != item.Id)
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
                if (!repo.FindBy(a => a.Id == id).Any())
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

        [HttpPost]
        public async Task<ActionResult<T>> Post(T item)
        {
            await repo.AddAsync(item);
            await repo.SaveChangesAsync();
            return CreatedAtAction(nameof(Get), new { id = item.Id }, item);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<T>> Delete(int id)
        {
            var item = await repo.GetAsync(id);
            if (item == null)
            {
                return NotFound();
            }

            await repo.DeleteAsync(item.Id);
            await repo.SaveChangesAsync();
            return item;
        }
    }
}