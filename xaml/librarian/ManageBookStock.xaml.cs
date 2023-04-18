using LibrarySystem.classes;
using LibrarySystem.xaml;
using LibrarySystem.xaml.librarian;
using System;
using System.Collections.Generic;
using System.Globalization; //Needed for the date format to be correct (date-month-year as opposed to month-date-year)
using System.Linq;
using System.Runtime.CompilerServices;
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
    //ListBox.ItemContainerStyle solution found at https://stackoverflow.com/a/20934416
    public partial class ManageBookStock : Page
    {
        private Book _thisBook;

        private class checkBookDisplay //Used for the items source binding
        {
            public string cardNumber { get; set; }
            public string name { get; set; }
            public string dueDate { get; set; }
            public string fontWeight { get; set; }
            public string fontStyle { get; set; }
            public string notifyIsEnabled { get; set; }
        }

        public ManageBookStock(Book thisBook)
        {
            InitializeComponent();
            _thisBook = thisBook;
            lblisbn.Content = String.Format("Current Stock Level ({0}):", _thisBook._ISBN);
            UpdateDisplay();
        }

        private void UpdateDisplay()
        {
            XmlController controller = new XmlController();
            _thisBook = controller.GetLibrary(_thisBook._ISBN, "isbn")[0];
            txtStock.Text = String.Format("{0} / {1}", _thisBook.currentStock, _thisBook._totalStock);

            List<checkBookDisplay> checkBookList = new List<checkBookDisplay>();
            List<checkBookDisplay> queueBookList = new List<checkBookDisplay>();
            DateTimeOffset currTime = DateTimeUK.DateTimeOffsetNow; //Keeps the current time as a constant value in the for loop

            int deletedMemberIndex = 0;
            for (int i = 0; i < _thisBook.checkoutListLength; i++)
            {
                string thisFontWeight = "Regular";
                string thisFontStyle = "Normal";
                string thisNotifyIsEnabled = "False";
                if (currTime >= _thisBook.getDueDate(_thisBook.getCardNumber(i), deletedMemberIndex))
                {
                    thisFontWeight = "Bold";
                    thisNotifyIsEnabled = "True";
                }
                string thisMemberName = controller.GetMemberName(_thisBook.getCardNumber(i));
                if (thisMemberName == null)
                {
                    deletedMemberIndex++;
                    thisMemberName = "This person is no longer a member";
                    thisFontStyle = "Italic";
                    thisNotifyIsEnabled = "False";
                }
                if (_thisBook.getDueDate(_thisBook.getCardNumber(i)) != DateTimeOffset.FromUnixTimeSeconds(0))
                {
                    checkBookList.Add(new checkBookDisplay
                    {
                        cardNumber = _thisBook.getCardNumber(i),
                        name = thisMemberName,
                        dueDate = _thisBook.getDueDate(_thisBook.getCardNumber(i), deletedMemberIndex - 1).DateTime.ToString(CultureInfo.CreateSpecificCulture("en-GB")),
                        fontWeight = thisFontWeight,
                        fontStyle = thisFontStyle,
                        notifyIsEnabled = thisNotifyIsEnabled
                    });
                }
                else
                {
                    queueBookList.Add(new checkBookDisplay
                    {
                        cardNumber = _thisBook.getCardNumber(i),
                        name = controller.GetMemberName(_thisBook.getCardNumber(i))
                    });
                }
            }
            listChecked.ItemsSource = checkBookList;
            listQueued.ItemsSource = queueBookList;

            btnMoveQueue.IsEnabled = false;
            if (_thisBook.queueForBook > 0)
            {
                btnMoveQueue.IsEnabled = true;
            }
            if (_thisBook.getIndexFromCardNumber("0") != -1)
            {
                //The button is only enabled if there are missing books in the checkout. If not, there is no point having it enabled.
                btnDeleteMissingBooks.IsEnabled = true;
            }
        }

        private void btnNotify_Click(object sender, RoutedEventArgs e)
        {
            checkBookDisplay thisBook = (checkBookDisplay)((Button)sender).DataContext;
            OverdueBooks.notifyMember(thisBook.cardNumber, _thisBook._title, _thisBook._ISBN);
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private void btnDeleteMissingBooks_Click(object sender, RoutedEventArgs e)
        {
            DeleteMissingBooks deleteMissingBooks = new DeleteMissingBooks(_thisBook);
            deleteMissingBooks.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            deleteMissingBooks.ShowDialog();
            UpdateDisplay();
        }

        private void btnMoveQueue_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult confirm = MessageBox.Show("Are you sure you want to check the book out for the queued members?", "Move Queued Members", MessageBoxButton.YesNo);
            if (confirm == MessageBoxResult.Yes)
            {
                List<string> notifyMemberCardNumbers = _thisBook.moveQueueToCheckout();
                XmlController controller = new XmlController();
                controller.UpdateBookCheckout(_thisBook);
                foreach (string cardNumber in notifyMemberCardNumbers)
                {
                    controller.NotifyMember(cardNumber, String.Format("A librarian has checked you out for the book {0} (ISBN: {1}). It is due in on {2}. Please pick it up from the library as soon as you are able to!", _thisBook._title, _thisBook._ISBN, _thisBook.getDueDate(cardNumber).DateTime.ToString(CultureInfo.CreateSpecificCulture("en-GB"))));
                }
                UpdateDisplay();
                if (notifyMemberCardNumbers == null)
                {
                    MessageBox.Show("Could not move all the queued members, book stock is too low.", "Move Queued Members");
                }
            }
        }

        private void btnChangeStock_Click(object sender, RoutedEventArgs e)
        {
            ChangeValue changeValue = new ChangeValue(_thisBook);
            changeValue.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            if (changeValue.ShowDialog() == true)
            {
                UpdateDisplay();
                XmlController controller = new XmlController();
                int i = 0;
                foreach (checkBookDisplay member in listQueued.ItemsSource)
                {
                    if (i < _thisBook.currentStock)
                    {
                        controller.NotifyMember(member.cardNumber, String.Format("Stock for the book {0} (ISBN {1}) has changed, you can check the book out!", _thisBook._title, _thisBook._ISBN));
                        i++;
                    }
                    else { break; }
                }
            }
        }
    }
}
