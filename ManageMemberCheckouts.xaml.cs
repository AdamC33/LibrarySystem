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

            List<CheckBookDisplay> checkBookList = new List<CheckBookDisplay>();
            List<CheckBookDisplay> queueBookList = new List<CheckBookDisplay>();
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
                            string thisNotifyIsEnabled = "False";
                            if (currTime > b.getDueDate(_thisMember._cardNumber))
                            {
                                thisFontWeight = "Bold";
                                thisNotifyIsEnabled = "True";
                            }
                            //Books that the member has checked out
                            checkBookList.Add(new CheckBookDisplay
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
                            queueBookList.Add(new CheckBookDisplay
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

        private void btnNotify_Click(object sender, RoutedEventArgs e)
        {
            CheckBookDisplay thisBook = (CheckBookDisplay)((Button)sender).DataContext;
            XmlController controller = new XmlController();
            if (controller.NotifyMember(_thisMember._cardNumber, String.Format("{0} ({1}) is past its due date and you have still not returned it - please return it!", thisBook.title, thisBook.ISBN)))
            {
                MessageBox.Show("Successfully notified the member!", "Notify Member");
            }
            else
            {
                MessageBox.Show("Could not notify the member!", "Notify Member");
            }
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}
