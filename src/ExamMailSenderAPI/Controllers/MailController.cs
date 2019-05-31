using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExamMailSenderAPI.Models;
using ExamMailSenderAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace ExamMailSenderAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MailController : ControllerBase
    {
        private readonly IConfiguration configuration;
        private IMailService Mail;
        public MailController(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        [HttpPost]
        public async Task<IActionResult> Post(MailModel model)
        {
            if (ModelState.IsValid)
            {
                Mail = new MailService(model, configuration);
                string message = await Mail.Send();
                if (message == "Wysłano")
                    return Ok(message);
                return StatusCode(500, message);
            }
            return BadRequest(model);
        }
    }
}