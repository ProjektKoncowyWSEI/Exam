using ExamMailSenderAPI.Models;
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
        private readonly MailModel model;
        private readonly IConfiguration configuration;

        public MailService(MailModel model, IConfiguration configuration)
        {
            this.model = model;
            this.configuration = configuration;
        }
        public async Task<string> Send()
        {
            try
            {
                IConvertable converter = new Converter();
                var apiKey = configuration.GetSection("SENDGRID_API_KEY").Value;
                var client = new SendGridClient(apiKey);
                var from = new EmailAddress(model.From, model.From);
                List<EmailAddress> tos = new List<EmailAddress>();
                model.To.ForEach(m => tos.Add(new EmailAddress(m)));

                var msg = MailHelper.CreateSingleEmailToMultipleRecipients(from, tos, model.Title, model.Body, model.HtmlBody, true);
                model.Attachments.ToList().ForEach(a =>
                {
                    msg.AddAttachment(
                        new Attachment()
                        {
                            Content = converter.GetStringBase64(a.Content),
                            Disposition = "attachment",
                            Filename = a.FileName
                        });
                });
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
