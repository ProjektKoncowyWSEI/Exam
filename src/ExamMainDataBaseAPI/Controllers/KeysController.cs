using ExamContract.Auth;

namespace ExamMainDataBaseAPI.Controllers
{
    public class KeysController : KeyApiController
    {
        public KeysController(ApiKeyRepo repo) : base(repo)
        {
        }
    }
}