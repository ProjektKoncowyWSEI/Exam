using ExamContract.MainDbModels;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Exam.Services
{
    public class AnswersApiClient : WebApiClient<Answer>
    {
        public AnswersApiClient(ILogger logger, IConfiguration configuration) : base(logger, configuration, "MainDbAPIConnection", "Answers", "Exam_MainDbApiKey")
        {
        }
    }
}
