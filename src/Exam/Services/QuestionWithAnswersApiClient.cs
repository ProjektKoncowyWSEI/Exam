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
    public class QuestionWithAnswersApiClient : WebApiClient<Question>
    {
        public QuestionWithAnswersApiClient(ILogger logger, IConfiguration configuration) : base(logger, configuration, "MainDbAPIConnection", "QuestionWithAnswers")
        {
        }       
    }
}
