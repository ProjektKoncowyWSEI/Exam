//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using ExamTutorialsAPI.Models;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;

//namespace ExamTutorialsAPI.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]

//    public class TutorialsController : ControllerBase
//    {
//        private readonly ExamTutorialsApiContext db;
//        public TutorialsController(ExamTutorialsApiContext db)
//        {
//            this.db = db;
//        }
//        // GET: api/Tutorial
//        [HttpGet]
//        public IActionResult GetTutorials()
//        {
//            try
//            {
//                return Ok(db.Tutorials.ToList());

//            }
//            catch (Exception ex)
//            {
//                return BadRequest(ex);
//            }

//        }

//        // GET: api/Tutorial/5
//        [HttpGet("{id}")]
//        public IActionResult GetTutorials(int id)
//        {
//            try
//            {
//                var tutorial = db.Tutorials.Include(a => a.Category).SingleOrDefault(a => a.Id == id);
//                if (tutorial == null)
//                {
//                    return NotFound($"Id:{id} wasn't found");

//                }
//                return Ok(tutorial);
//            }
//            catch (Exception ex)
//            {
//                return BadRequest(ex);
//            }
//        }

//        // POST: api/Tutorial
//        [HttpPost]
//        public IActionResult AddTutorials(Tutorial tutorial)
//        {
//            try
//            {
//                if (ModelState.IsValid && CategoryExists(tutorial.CategoryId))
//                {
//                    db.Tutorials.Add(tutorial);
//                    db.SaveChanges();

//                    return CreatedAtAction(nameof(GetTutorials), new { id = tutorial.Id }, tutorial);
//                }
//                return BadRequest("Invalid input data");
//            }
//            catch (Exception ex)
//            {
//                return BadRequest(ex);
//            }
//        }

//        // PUT: api/Tutorial/5
//        [HttpPut("{id}")]
//        public IActionResult UpdateTutorials(int id, Tutorial tutorial)
//        {
//            try
//            {
//                if (ModelState.IsValid && CategoryExists(tutorial.CategoryId))
//                {
//                    if (id < 0)
//                    {
//                        return BadRequest("Id can't be lover than 0");
//                    }
//                    var item = db.Tutorials.Find(id);
//                    if (item == null)
//                    {
//                        return NotFound();
//                    }
//                    item.Name = tutorial.Name;
//                    item.Image = tutorial.Image;
//                    item.Description = tutorial.Description;
//                    item.CategoryId = tutorial.CategoryId;
//                    db.Update(item);
//                    db.SaveChanges();
//                    return NoContent();
//                }
//                return BadRequest("Invalid input data");
//            }
//            catch (Exception ex)
//            {
//                return BadRequest(ex);
//            }
//        }


//        // DELETE: api/ApiWithActions/5
//        [HttpDelete("{id}")]
//        public IActionResult DeleteTutorials(int id)
//        {
//            try
//            {
//                var item = db.Tutorials.Find(id);
//                if (item==null)
//                {
//                    return NotFound();
//                }
//                db.Tutorials.Remove(item);
//                db.SaveChanges();
//                return Ok(item);
//            }
//            catch (Exception ex)
//            {
//                return BadRequest(ex);

//            }
//        }
//        private bool CategoryExists(int? id)
//        {
//            if (id == null)
//                return true;
//            return db.Categories.Any(a => a.Id == id);
//        }
//    }
//}

