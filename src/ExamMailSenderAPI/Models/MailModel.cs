using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExamMailSenderAPI.Models
{
    [Serializable]
    public class MailModel
    {
        public MailModel()
        {
            Attachments = new HashSet<Attachment>();
        }
        public int Id { get; set; }
        public string Title { get; set; }
        public string From { get; set; }
        public List<string> To { get; set; }
        public string Body { get; set; }
        public string HtmlBody { get; set; }
        public DateTime? Date { get; set; }
        public HashSet<Attachment> Attachments { get; set; }
        public class Attachment
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
        }
    }
}
