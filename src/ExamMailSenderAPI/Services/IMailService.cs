using ExamContract.MailingModels;
using System.Threading.Tasks;

namespace ExamMailSenderAPI.Services
{
    public interface IMailService
    {
        Task<string> Send(MailModel model);
    }
}
