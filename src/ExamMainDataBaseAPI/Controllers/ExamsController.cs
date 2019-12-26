using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ExamContract.MainDbModels;
using ExamMainDataBaseAPI.DAL;
using Helpers;
using ExamContract.Auth;

namespace ExamMainDataBaseAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [KeyAuthorize(RoleEnum.admin)]
    public class ExamsController : MyBaseController<Exam>
    {
        UnitOfWork uow;
        public ExamsController(UnitOfWork uow) : base(uow.ExamRepo)
        {
            this.uow = uow;
        }
        [HttpPost]          
        public override async Task<ActionResult<Exam>> Post(Exam item)
        {
            item.Code = await uow.GetNewGiud();
            return await base.Post(item);
        }

       
    }
}