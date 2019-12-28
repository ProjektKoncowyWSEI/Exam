using ExamContract.Auth;
using ExamContract.Auth;

namespace ExamMainDataBaseAPI.Controllers
{
    [KeyAuthorize(RoleEnum.admin)]
    public class KeysController : KeyApiController
    {
        public KeysController(ApiKeyRepo repo) : base(repo)
        {
        }
    }
}