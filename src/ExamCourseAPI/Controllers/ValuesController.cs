using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace ExamCourseAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new string[] { "To API działa" };
        }       
    }
}
