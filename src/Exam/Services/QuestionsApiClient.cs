using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Exam.Services
{
    public class QuestionsApiClient : WebApiClient<ExamContract.MainDbModels.Question>
    {
        public QuestionsApiClient(ILogger logger, IConfiguration configuration) : base(logger, configuration, "MainDbAPIConnection", "Questions", "Exam_MainDbApiKey")
        {
        }
    }
}
