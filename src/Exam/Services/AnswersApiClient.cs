using ExamContract.MainDbModels;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Exam.Services
{
    public class AnswersApiClient : WebApiClient<Answer>
    {
        public AnswersApiClient(ILogger logger, IConfiguration configuration) : base(logger, configuration, "MainDbAPIConnection", "Answers")
        {
        }
    }
}
