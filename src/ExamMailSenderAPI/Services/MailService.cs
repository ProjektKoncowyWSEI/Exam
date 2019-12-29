using ExamContract.MailingModels;
using Microsoft.Extensions.Configuration;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExamMailSenderAPI.Services
{
    public class MailService : IMailService
    {
        private readonly IConfiguration configuration;
        public MailService(IConfiguration configuration)
        {
            this.configuration = configuration;
        }
        public async Task<string> Send(MailModel model)
        {
            try
            {                               
                var apiKey = Environment.GetEnvironmentVariable("SENDGRID_API_KEY"); 
                var client = new SendGridClient(apiKey);
                var from = new EmailAddress(model.From, model.From);
                List<EmailAddress> tos = new List<EmailAddress>();
                model.To.ForEach(m => tos.Add(new EmailAddress(m)));

                var msg = MailHelper.CreateSingleEmailToMultipleRecipients(from, tos, model.Title, model.Body, model.HtmlBody, false);
                if (model.Attachments != null)
                {
                    model.Attachments.ToList().ForEach(a =>
                                   {
                                       msg.AddAttachment(
                                           new SendGrid.Helpers.Mail.Attachment()
                                           {
                                               Content = Convert.ToBase64String(a.Content),
                                               Disposition = "attachment",
                                               Filename = a.FileName
                                           });
                                   });
                }

                var response = await client.SendEmailAsync(msg);
                if (response.StatusCode == System.Net.HttpStatusCode.Accepted)
                    return "Wysłano";
                return response.StatusCode.ToString();
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }
}
