using ExamContract.TutorialModels;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Exam.Services
{
    public class TutorialsApiClient : WebApiClient<Tutorial>
    {
        public TutorialsApiClient(ILogger logger, IConfiguration configuration) : base(logger, configuration, "TutorialsAPIConnection", "Tutorials", "Exam_TutorialsApiKey")
        {
        }
    }
}
