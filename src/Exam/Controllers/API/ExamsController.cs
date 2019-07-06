//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using Exam.Services;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using EXAM = ExamContract.MainDbModels.Exam;

//namespace Exam.Controllers.API
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class ExamsController : ControllerBase
//    {
//        private readonly ExamsApiClient service;

//        public ExamsController(ExamsApiClient service)
//        {
//            this.service = service;
//        }
//        [HttpGet]
//        public async Task<ActionResult<List<EXAM>>> Get()
//        {
//            return await service.GetListAsync();
//        }

//        [HttpGet("{id}", Name = "Get")]
//        public async Task<ActionResult<EXAM>> Get(int id)
//        {
//            var item = await service.GetAsync(id);
//            if (item == null)
//                return NotFound();
//            return item;
//        }

//        [HttpPost]
//        public async Task<ActionResult<EXAM>> Post(EXAM item)
//        {
//            await service.AddAsync(item);
//            return CreatedAtAction(nameof(Get), new { id = item.Id }, item);
//        }

//        [HttpPut("{id}")]
//        public async Task<IActionResult> Put(int id, EXAM item)
//        {
//            if (id != item.Id)
//            {
//                return BadRequest();
//            }

//            try
//            {
//                await service.UpdateAsync(item);
//            }
//            catch (Exception)
//            {
//                if (await service.GetAsync(id) == null)
//                {
//                    return NotFound();
//                }
//                else
//                {
//                    throw;
//                }
//            }
//            return NoContent();
//        }

//        [HttpDelete("{id}")]
//        public async Task<ActionResult<EXAM>> Delete(int id)
//        {
//            var item = await service.GetAsync(id);
//            if (item == null)
//            {
//                return NotFound();
//            }
//            await service.DeleteAsync(item.Id);
//            return item;
//        }
//    }
//}