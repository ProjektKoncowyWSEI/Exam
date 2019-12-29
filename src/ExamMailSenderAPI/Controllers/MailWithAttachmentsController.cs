using System.Threading.Tasks;
using ExamContract.MailingModels;
using ExamMailSenderAPI.Data;
using Microsoft.AspNetCore.Mvc;

namespace ExamMailSenderAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MailWithAttachmentsController : ControllerBase
    {
        private readonly UnitOfWork uow;

        public MailWithAttachmentsController(UnitOfWork uow)
        {
            this.uow = uow;
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<MailModel>> Get(int id)
        {
            return await uow.GetMailWithAttachments(id);
        }
    }
}