using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExamContract.MailingModels
{
    public partial class MailModel : Entity
    {
        public MailModel()
        {
            Attachments = new HashSet<Attachment>();
        }
        public string Title { get; set; }
        public string From { get; set; }
        private string toStr;
        public string ToStr
        {
            get
            {

                return toStr ?? To.ListToString();
            }
            set
            {
                if (To != null && To.Count > 0)
                    toStr = To.ListToString();
                else
                    toStr = value;
            }
        }
        [NotMapped]
        public List<string> To { get; set; }
        public string Body { get; set; }
        public string HtmlBody { get; set; }
        public DateTime? Date { get; set; }
        public virtual ICollection<Attachment> Attachments { get; set; }
    }
}
