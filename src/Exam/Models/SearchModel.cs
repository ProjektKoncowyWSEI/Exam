using Helpers;

namespace Exam.Models
{
    public class SearchModel
    {
        public SearchModel(string searchIn, string searchWhat, string placeholder)
        {
            Autofocus = "autofocus";
            var guid = GlobalHelpers.GetShortGuid;
            SearchIn = searchIn;
            SearchWhat = searchWhat;
            SearchID = $"search{guid}";
            ClearID = $"clear{guid}";
            ClearOnClick = $"clearFilter('{SearchID}', '{ClearID}', '#{SearchIn}', '{SearchWhat}')";
            SearchOnInput = $"filter('{SearchID}', '{ClearID}', '#{SearchIn}', '{SearchWhat}')";
            SearchOnKeyDown = $"escapeSearch(event, '{SearchID}', '{ClearID}', '#{SearchIn}', '{SearchWhat}')";
            Placeholder = placeholder;
        }
        public string SearchID { get; set; }
        public string SearchIn { get; set; }
        public string SearchWhat { get; set; }
        public string SearchOnInput { get; set; }
        public string SearchOnKeyDown { get; set; }
        public string Placeholder { get; set; }
        public string Autofocus { get; set; }
        public string ClearID { get; set; }
        public string ClearOnClick { get; set; }
    }
}
