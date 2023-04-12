using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace LibrarySystem
{
    internal class OverdueBooks
    {
        public string memberCardNumber { get; set; }
        private string _memberName;
        public string bookISBN { get; set; }
        public string bookTitle { get; set; }
        private string dueDateString { get; set; }
        private string fontStyle { get; set; }
        private string btnNotifyIsEnabled { get; set; }

        public string memberName
        {
            get { return _memberName; }
            set
            {
                if (value == null)
                {
                    _memberName = "This person is no longer a member";
                    fontStyle = "Italic";
                    btnNotifyIsEnabled = "False";
                }
                else
                {
                    _memberName = value;
                    fontStyle = "Normal";
                    btnNotifyIsEnabled = "True";
                }
            }
        }

        public DateTimeOffset dueDate
        {
            set { dueDateString = value.DateTime.ToString(CultureInfo.CreateSpecificCulture("en-GB")); }
        }

        public string getDueDate
        {
            get { return dueDateString; }
        }

        public string getFontStyle
        {
            get { return fontStyle; }
        }

        public string getBtnNotifyIsEnabled
        {
            get { return btnNotifyIsEnabled; }
        }

        internal static void notifyMember(string memberCardNumber, string bookTitle, string bookISBN)
        {
            XmlController controller = new XmlController();
            if (controller.NotifyMember(memberCardNumber, String.Format("{0} ({1}) is past its due date and you have still not returned it - please return it!", bookTitle, bookISBN)))
            {
                MessageBox.Show("Successfully notified the member!", "Notify Member");
            }
            else
            {
                MessageBox.Show("Could not notify the member!", "Notify Member");
            }
        }
    }
}
