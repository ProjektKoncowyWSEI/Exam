using System;
using System.Threading.Tasks;
using ExamContract.MailingModels;
using ExamMailSenderAPI.Data;
using ExamMailSenderAPI.Services;
using Microsoft.AspNetCore.Mvc;

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