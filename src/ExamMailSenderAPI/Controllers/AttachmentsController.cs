using Microsoft.AspNetCore.Mvc;
using ExamMailSenderAPI.Data;
using ExamContract.MailingModels;

namespace ExamMailSenderAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AttachmentsController : MyBaseController<Attachment>
    {
        public AttachmentsController(Repository<Attachment> repo) : base(repo)
        {           
        }   
    }
}
