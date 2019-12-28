using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using ExamContract;
using ExamContract.Auth;
using ExamMainDataBaseAPI.DAL;
using ExamContract.Auth;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ExamMainDataBaseAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public abstract class MyBaseController<T> : ControllerBase where T : Entity
    {
        private readonly Repository<T> repo;

        public MyBaseController(Repository<T> repository)
        {
            repo = repository;
        }
        [HttpGet]
        [KeyAuthorize(RoleEnum.admin, RoleEnum.teacher, RoleEnum.student)]
        public virtual async Task<ActionResult<IEnumerable<T>>> Get(string login, bool? onlyActive = null)
        {
            Expression<Func<T, bool>> predicate;
            if (onlyActive == true && login == null)            
                predicate = a => a.Active == true;            
            else if (onlyActive != true && login != null)            
                predicate = a => a.Login == login;            
            else if (onlyActive == true && login != null)            
                predicate = a => a.Active == true && a.Login == login;            
            else
                return await repo.GetListAsync();
            return await repo.FindBy(predicate).ToListAsync();
        }

        [HttpGet("{id}")]
        [KeyAuthorize(RoleEnum.admin, RoleEnum.teacher, RoleEnum.student)]
        public virtual async Task<ActionResult<T>> Get(int id)
        {
            var item = await repo.GetAsync(id);
            if (item == null)
                return NotFound();
            return item;
        }

        [HttpPut("{id}")]
        [KeyAuthorize(RoleEnum.admin, RoleEnum.teacher)]
        public virtual async Task<IActionResult> Put(int id, T item)
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
        [KeyAuthorize(RoleEnum.admin, RoleEnum.teacher)]
        public virtual async Task<ActionResult<T>> Post(T item)
        {
            await repo.AddAsync(item);
            await repo.SaveChangesAsync();
            return CreatedAtAction(nameof(Get), new { id = item.Id }, item);
        }

        [HttpDelete("{id}")]
        [KeyAuthorize(RoleEnum.admin, RoleEnum.teacher)]
        public virtual async Task<ActionResult<T>> Delete(int id)
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