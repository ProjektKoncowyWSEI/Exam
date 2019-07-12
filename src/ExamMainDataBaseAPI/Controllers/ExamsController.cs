using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ExamContract.MainDbModels;
using ExamMainDataBaseAPI.Models;
using ExamMainDataBaseAPI.DAL;
using Helpers;

namespace ExamMainDataBaseAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
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