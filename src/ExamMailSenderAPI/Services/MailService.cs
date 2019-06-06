using ExamContract.MailingModels;
using FileConverter;
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
                IConvertable converter = new Converter();
                //var apiKey = configuration.GetSection("SENDGRID_API_KEY").Value; Jeśli chcemy trzymać w konfiguracji
                var apiKey = Environment.GetEnvironmentVariable("SENDGRID_API_KEY"); // Jeśli chcemy mieć zapamiętane w systemie
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
                                               Content = converter.GetStringBase64(a.Content),
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
