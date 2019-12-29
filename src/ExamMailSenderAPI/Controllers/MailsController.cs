using Microsoft.AspNetCore.Mvc;
using ExamMailSenderAPI.Data;
using ExamContract.MailingModels;

namespace ExamMailSenderAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MailsController : MyBaseController<MailModel>
    {   
        public MailsController(Repository<MailModel> repo) : base(repo)
        { 
        }   
    }
}
