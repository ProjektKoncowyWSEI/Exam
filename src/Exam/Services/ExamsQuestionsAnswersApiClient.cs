using ExamContract.MainDbModels;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Exam.Services
{
    public class ExamsQuestionsAnswersApiClient : WebApiClient<ExamContract.MainDbModels.Exam>
    {
        public ExamsQuestionsAnswersApiClient(ILogger logger, IConfiguration configuration) : base(logger, configuration, "MainDbAPIConnection", "ExamsQuestionsAnswers")
        {
        }       
    }
}
