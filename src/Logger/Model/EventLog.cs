using System;
using System.ComponentModel.DataAnnotations;

namespace Logger.Model
{
    public class EventLog
    {
        [Key]
        public int id { get; set; }
        public string Message { get; set; }
        public int EventId { get; set; }
        public string LogLevel { get; set; }
        public DateTime CreatedTime { get; set; }
    }
}
