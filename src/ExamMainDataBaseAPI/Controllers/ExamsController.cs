using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ExamContract.MainDbModels;
using ExamMainDataBaseAPI.DAL;
using ExamContract.Auth;


namespace ExamMainDataBaseAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [KeyAuthorize(RoleEnum.admin, RoleEnum.teacher)]
    public class ExamsController : MyBaseController<Exam>
    {
        UnitOfWork uow;
        public ExamsController(UnitOfWork uow) : base(uow.ExamRepo)
        {
            this.uow = uow;
        }
        [HttpPost]
        [KeyAuthorize(RoleEnum.admin, RoleEnum.teacher)]
        public override async Task<ActionResult<Exam>> Post(Exam item)
        {
            item.Code = await uow.GetNewGiud();
            return await base.Post(item);
        }
    }
}