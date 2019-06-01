using ExamMailSenderAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExamMailSenderAPI.Services
{
    public interface IMailService
    {
        Task<string> Send(MailModel model);
    }
}
