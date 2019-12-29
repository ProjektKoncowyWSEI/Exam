using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Exam.Services
{
    public class ExamsApiClient : WebApiClient<ExamContract.MainDbModels.Exam>
    {
        public ExamsApiClient(ILogger logger, IConfiguration configuration) : base(logger, configuration, "MainDbAPIConnection", "Exams", "Exam_MainDbApiKey")
        {
        }
    }
}
