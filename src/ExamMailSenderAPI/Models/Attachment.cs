using ExamMailSenderAPI.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExamMailSenderAPI.Models
{
    public partial class Attachment : Entity
    {
        public Attachment()
        {
        }
        public Attachment(string fileName, byte[] content)
        {
            FileName = fileName;
            Content = content;
        }      
        public string FileName { get; set; }
        public byte[] Content { get; set; }
        public int MailModelId { get; set; }
    }
}
