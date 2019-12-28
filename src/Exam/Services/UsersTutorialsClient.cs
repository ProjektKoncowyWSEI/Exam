using ExamContract.TutorialModels;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Exam.Services
{
    public class UsersTutorialsClient : WebApiClient<User>
    {
        public UsersTutorialsClient(ILogger logger, IConfiguration configuration) : base(logger, configuration, "TutorialsAPIConnection", "Users", "Exam_TutorialsApiKey")
        {
        }
    }
}
