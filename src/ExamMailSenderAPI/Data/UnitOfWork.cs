using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExamContract.Auth;
using Microsoft.EntityFrameworkCore;
using ExamContract.MailingModels;
using Microsoft.Extensions.Logging;

namespace ExamMailSenderAPI.Data
{
    public class UnitOfWork
    {
        private readonly Repository<MailModel> mailsRepo;
        private readonly Repository<Attachment> attachmentsRepo;
        private readonly ILogger logger;

        public UnitOfWork(Repository<MailModel> mailsRepo, Repository<Attachment> attachmentsRepo, ILogger logger)
        {
            this.mailsRepo = mailsRepo;
            this.attachmentsRepo = attachmentsRepo;
            this.logger = logger;
        }
        public async Task<bool> SaveMailWithAttachments(MailModel item)
        {
            try
            {
                await mailsRepo.AddAsync(item);
                foreach (var att in item.Attachments)
                {
                    await attachmentsRepo.AddAsync(att);
                }
                await mailsRepo.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, System.Reflection.MethodBase.GetCurrentMethod().ToString());
                throw;
            }
        }
        public async Task<MailModel> GetMailWithAttachments(int id)
        {
            var mail = await mailsRepo.GetAsync(id);
            var attachments = await attachmentsRepo.FindBy(a => a.MailModelId == id).ToListAsync();
            mail.Attachments = attachments;
            return mail;
        }

    }
}
