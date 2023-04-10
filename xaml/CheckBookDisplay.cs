using LibrarySystem.classes;
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

        static internal List<CheckBookDisplay>[] GetMemberLibraryDisplay(Member thisMember)
        {
            XmlController controller = new XmlController();

            List<CheckBookDisplay>[] memberLibraryList = new List<CheckBookDisplay>[2];
            memberLibraryList[0] = new List<CheckBookDisplay>();
            memberLibraryList[1] = new List<CheckBookDisplay>();
            DateTimeOffset currTime = DateTimeUK.DateTimeOffsetNow; //Keeps the current time as a constant value in the for loop

            List<Book> library = controller.GetLibrary();

            foreach (Book b in library)
            {
                for (int i = 0; i < b.checkoutListLength; i++)
                {
                    if (b.getCardNumber(i) == thisMember._cardNumber)
                    {
                        if (b.getDueDate(thisMember._cardNumber) != DateTimeOffset.FromUnixTimeSeconds(0))
                        {
                            string thisFontWeight = "Regular";
                            string thisNotifyIsEnabled = "False";
                            if (currTime > b.getDueDate(thisMember._cardNumber))
                            {
                                thisFontWeight = "Bold";
                                thisNotifyIsEnabled = "True";
                            }
                            //Books that the member has checked out
                            memberLibraryList[0].Add(new CheckBookDisplay
                            {
                                ISBN = b._ISBN,
                                title = b._title,
                                dueDate = b.getDueDate(b.getCardNumber(i)),
                                fontWeight = thisFontWeight,
                                notifyIsEnabled = thisNotifyIsEnabled,
                            });
                        }
                        else
                        {
                            //Books that the member is queued for
                            memberLibraryList[1].Add(new CheckBookDisplay
                            {
                                ISBN = b._ISBN,
                                title = b._title
                            });
                        }
                    }
                }
            }

            return memberLibraryList;
        }
    }
}
