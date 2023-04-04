using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarySystem
{
    //Used for both the ManageMemberCheckouts.xaml list display and the MemberAccount.xaml list display

    internal class CheckBookDisplay
    {
        public string ISBN { get; set; }
        public string title { get; set; }
        private DateTimeOffset dueDateDateTime { get; set; }
        private string dueDateString { get; set; }
        public string fontWeight { get; set; }
        public string notifyIsEnabled { get; set; }

        public DateTimeOffset dueDate
        {
            set
            {
                dueDateDateTime = value;
                dueDateString = value.DateTime.ToString(CultureInfo.CreateSpecificCulture("en-GB"));
            }
        }

        public DateTimeOffset getDueDate
        {
            get { return dueDateDateTime; }
        }

        public string getDueDateString
        {
            get { return dueDateString; }
        }
    }
}
