using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Exam.Models
{
    public class SearchModel
    {
        public SearchModel()
        {
            Autofocus = "autofocus";
        }
        public string SearchID { get; set; }
        public string SearchOnInput { get; set; }
        public string Placeholder { get; set; }
        public string Autofocus { get; set; }
        public string ClearID { get; set; }
        public string ClearOnClick { get; set; }
    }
}
