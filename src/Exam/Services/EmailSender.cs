using ExamContract;
using ExamContract.MailingModels;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Exam.Services
{
    public class EmailSender : WebApiClient<Entity>, IEmailSender
    {
        public bool IsSent { get; set; }
        public EmailSender(ILogger logger, IConfiguration configuration) : base(logger, configuration, "EmailSenderConnection", "", "Exam_mailApiKey")                               
        {
            this.logger = logger;
        }
        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            var input = new MailModel
            {
                Id = 0,
                Title = subject,
                To = new List<string> { email },
                From = "exam@exam.pl",
                HtmlBody = htmlMessage,
                Date = DateTime.Now,
                Attachments = null
            };
            try
            {
                var response = await Client.PostAsJsonAsync(Client.BaseAddress, input);
                if (response.IsSuccessStatusCode)
                {
                    IsSent = true;
                }
                IsSent = false;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, System.Reflection.MethodBase.GetCurrentMethod().ToString());
                IsSent = false;
            }
        }
    }
}
