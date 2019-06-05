using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
