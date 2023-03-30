using System;
using System.Collections.Generic;
using System.Globalization; //Needed for the date format to be correct (date-month-year as opposed to month-date-year). Same as ManageBookStock
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace LibrarySystem
{
    /// <summary>
    /// Interaction logic for ManageMemberCheckouts.xaml
    /// </summary>
    public partial class ManageMemberCheckouts : Page
    {
        private Member _thisMember;

        private class checkBookDisplay
        {
            public string ISBN { get; set; }
            public string title { get; set; }
            private DateTimeOffset dueDateDateTime { get; set; }
            private string dueDateString { get; set; }
            public string fontWeight { get; set; }

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

        public ManageMemberCheckouts(Member thisMember)
        {
            InitializeComponent();
            _thisMember = thisMember;
            lblCardNumber.Content = String.Format("Member {0}'s Library:", _thisMember._cardNumber);
            UpdateDisplay();
        }

        private void UpdateDisplay()
        {
            XmlController controller = new XmlController();

            List<checkBookDisplay> checkBookList = new List<checkBookDisplay>();
            List<checkBookDisplay> queueBookList = new List<checkBookDisplay>();
            DateTimeOffset currTime = DateTimeOffset.Now; //Keeps the current time as a constant value in the for loop

            List<Book> library = controller.GetLibrary();

            foreach (Book b in library)
            {
                for (int i = 0; i < b.checkoutListLength; i++)
                {
                    if (b.getCardNumber(i) == _thisMember._cardNumber)
                    {
                        if (b.getDueDate(_thisMember._cardNumber) != DateTimeOffset.FromUnixTimeSeconds(0))
                        {
                            string thisFontWeight = "Regular";
                            if (currTime > b.getDueDate(_thisMember._cardNumber))
                            {
                                thisFontWeight = "Bold";
                            }
                            //Books that the member has checked out
                            checkBookList.Add(new checkBookDisplay
                            {
                                ISBN = b._ISBN,
                                title = b._title,
                                dueDate = b.getDueDate(b.getCardNumber(i)),
                                fontWeight = thisFontWeight
                            });
                        }
                        else
                        {
                            //Books that the member is queued for
                            queueBookList.Add(new checkBookDisplay
                            {
                                ISBN = b._ISBN,
                                title = b._title
                            });
                        }
                    }
                }
            }

            listChecked.ItemsSource = checkBookList.OrderBy(o=>o.getDueDate);
            listQueued.ItemsSource = queueBookList;
        }

        private void btnReturn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnRenew_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}
