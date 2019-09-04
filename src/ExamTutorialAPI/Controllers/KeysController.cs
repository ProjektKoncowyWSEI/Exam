using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExamContract.TutorialModels;
using ExamTutorialsAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ExamTutorialsAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class KeysController : ControllerBase
    {

        private readonly Context _context;
        public KeysController (Context context)
        {
            _context = context;
        }
        // GET: api/Keys
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Key>>> GetKeys()
        {
            return await _context.Keys.ToListAsync();
        }

        // GET: api/Keys/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Key>> GetKey(string id)
        {
            var key = await _context.Keys.FindAsync(id);
            if (key == null)
            {
                return NotFound();
            }
            return key;
        }

        // PUT: api/Keys/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutKey(string id, Key key)
        {
            if (id != key.Name)
            {
                return BadRequest();
            }
            _context.Entry(key).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!KeyExists(id))
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
       

        // POST: api/Keys
        [HttpPost]
        public async Task<ActionResult<Key>> PostKey(Key key)
        {
            _context.Keys.Add(key);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetKey", new { id = key.Name }, key);
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Key>> DeleteKey(string id)
        {
            var key = await _context.Keys.FindAsync(id);
            if (key ==null)
            {
                return NotFound();
            }
            _context.Keys.Remove(key);
            await _context.SaveChangesAsync();

            return key;
        }

        private bool KeyExists(string id)
        {
            return _context.Keys.Any(e => e.Name == id);
        }
    }
}
