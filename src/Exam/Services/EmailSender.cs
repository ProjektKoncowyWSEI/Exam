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
    public class EmailSender : IEmailSender
    {
        HttpClient client = new HttpClient();
        private readonly ILogger logger;

        public bool IsSent { get; set; }
        public EmailSender(IConfiguration configuration)
        {
            client.BaseAddress = new Uri(configuration.GetConnectionString("EmailSenderConnection"));
            //client.BaseAddress = new Uri("https://localhost:44371/api/sendmail");
            client.DefaultRequestHeaders
                .Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Add("api-key", configuration.GetValue<string>("MailKey"));            
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
                var response = await client.PostAsJsonAsync(client.BaseAddress, input);
                if (response.IsSuccessStatusCode)
                {
                    IsSent = true;
                }
                IsSent = false;
            }
            catch (Exception ex)
            {
                IsSent = false;               
            }
        }
    }
}
