using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Exam.Services
{
    public class ExamApproachesApiClient : WebApiClient<ExamContract.MainDbModels.Exam>
    {
        public ExamApproachesApiClient(ILogger logger, IConfiguration configuration) : base(logger, configuration, "MainDbAPIConnection", "ExamApproaches")
        {
        }
    }
}
