using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Helpers;
using Microsoft.EntityFrameworkCore;
using ExamContract.MailingModels;

namespace ExamMailSenderAPI.Data
{
    public class UnitOfWork
    {
        private readonly Repository<MailModel> mailsRepo;
        private readonly Repository<Attachment> attachmentsRepo;

        public UnitOfWork(Repository<MailModel> mailsRepo, Repository<Attachment> attachmentsRepo)
        {
            this.mailsRepo = mailsRepo;
            this.attachmentsRepo = attachmentsRepo;
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
