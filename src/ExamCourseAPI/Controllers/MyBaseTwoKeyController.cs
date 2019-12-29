using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExamContract.CourseModels;
using ExamCourseAPI.DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ExamCourseAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MyBaseTwoKeyController<T> : ControllerBase where T : class, ICourseTwoKey
    {
        private readonly TwoKeysRepository<T> repo;

        public MyBaseTwoKeyController(TwoKeysRepository<T> repo)
        {
            this.repo = repo;
        }
        [HttpGet]
        public virtual async Task<ActionResult<IEnumerable<T>>> Get(int? courseId = null, int? id = null)
        {
            if (courseId == null && id != null)
                return await repo.GetListAsync(null, (int)id);
            if (courseId != null && id == null)
                return await repo.GetListAsync((int)courseId);
            else if (courseId != null && id != null)
                return await repo.GetListAsync(courseId, (int)id);
            else
                return await repo.GetListAsync();
        }

        [HttpGet("{course}/{id}")]
        public virtual async Task<ActionResult<T>> Get(int course, int id)
        {
            var item = await repo.GetAsync(course, id);
            if (item == null)
                return NotFound();
            return item;
        }

        [HttpPut("{id}")]
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
        public virtual async Task<ActionResult<T>> Post(T item)
        {
            await repo.AddAsync(item);
            await repo.SaveChangesAsync();
            return CreatedAtAction(nameof(Get), new { course = item.CourseId, id = item.Id }, item);
        }

        [HttpDelete("{course}/{id}")]
        public virtual async Task<ActionResult<T>> Delete(int course, int id)
        {
            var item = await repo.GetAsync(course, id);
            if (item == null)
            {
                return NotFound();
            }

            await repo.DeleteAsync(item);
            await repo.SaveChangesAsync();
            return item;
        }
    }
}