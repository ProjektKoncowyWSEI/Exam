using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExamMailSenderAPI.Data;
using ExamMailSenderAPI.Models;
using ExamMailSenderAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace ExamMailSenderAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SendMailController : ControllerBase
    {
        private readonly IMailService Mail;
        private readonly UnitOfWork uow;

        public SendMailController(IMailService mail, UnitOfWork uow)
        {
            Mail = mail;
            this.uow = uow;
        }

        [HttpPost]
        public async Task<IActionResult> Post(MailModel model)
        {
            if (ModelState.IsValid)
            {
                string message = await Mail.Send(model);
                if (message == "Wysłano")
                {
                    try
                    {
                        await uow.SaveMailWithAttachments(model);
                    }
                    catch (Exception)
                    {
                        message += " - Nie udało się zapisać";
                    }
                    return Ok(message);
                }

                return StatusCode(500, message);
            }
            return BadRequest(model);
        }
    }
}