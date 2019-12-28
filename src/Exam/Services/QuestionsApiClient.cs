using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Exam.Services
{
    public class QuestionsApiClient : WebApiClient<ExamContract.MainDbModels.Question>
    {
        public QuestionsApiClient(ILogger logger, IConfiguration configuration) : base(logger, configuration, "MainDbAPIConnection", "Questions", "Exam_MainDbApiKey")
        {
        }
    }
}
