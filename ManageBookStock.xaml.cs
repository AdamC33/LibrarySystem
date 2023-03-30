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
            DateTimeOffset currTime = DateTimeOffset.Now; //Keeps the current time as a constant value in the for loop

            for (int i = 0; i < _thisBook.checkoutListMinusQueueLength; i++)
            {
                string thisFontWeight = "Regular";
                string thisNotifyIsEnabled = "False";
                if (currTime >= _thisBook.getDueDate(_thisBook.getCardNumber(i)))
                {
                    thisFontWeight = "Bold";
                    thisNotifyIsEnabled = "True";
                }
                checkBookList.Add(new checkBookDisplay
                {
                    cardNumber = _thisBook.getCardNumber(i),
                    name = controller.GetMemberName(_thisBook.getCardNumber(i)),
                    dueDate = _thisBook.getDueDate(_thisBook.getCardNumber(i)).DateTime.ToString(CultureInfo.CreateSpecificCulture("en-GB")),
                    fontWeight = thisFontWeight,
                    notifyIsEnabled = thisNotifyIsEnabled
                });
            }
            listChecked.ItemsSource = checkBookList;
            List<checkBookDisplay> queueBookList = new List<checkBookDisplay>();
            for (int i = _thisBook.checkoutListMinusQueueLength; i < _thisBook.checkoutListLength; i++)
            {
                queueBookList.Add(new checkBookDisplay
                {
                    cardNumber = _thisBook.getCardNumber(i),
                    name = controller.GetMemberName(_thisBook.getCardNumber(i))
                });
            }
            listQueued.ItemsSource = queueBookList;

            btnMoveQueue.IsEnabled = false;
            if (_thisBook.queueForBook > 0)
            {
                btnMoveQueue.IsEnabled = true;
            }
        }

        private void btnNotify_Click(object sender, RoutedEventArgs e)
        {
            checkBookDisplay thisBook = (checkBookDisplay)((Button)sender).DataContext;
            XmlController controller = new XmlController();
            if (controller.NotifyMember(thisBook.cardNumber, String.Format("{0} ({1}) is past its due date and you have still not returned it - please return it!", _thisBook._title, _thisBook._ISBN)))
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

        private void btnMoveQueue_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult confirm = MessageBox.Show("Are you sure you want to check the book out for the queued members?", "Move Queued Members", MessageBoxButton.YesNo);
            if (confirm == MessageBoxResult.Yes)
            {
                bool movedEverything = _thisBook.moveQueueToCheckout();
                XmlController controller = new XmlController();
                controller.UpdateBookCheckout(_thisBook);
                UpdateDisplay();
                if (!movedEverything)
                {
                    MessageBox.Show("Could not move all the queued members, book stock is too low.", "Move Queued Members");
                }
            }
        }

        private void btnChangeStock_Click(object sender, RoutedEventArgs e)
        {
            ChangeValue changeValue = new ChangeValue(_thisBook);
            if (changeValue.ShowDialog() == true)
            {
                UpdateDisplay();
            }
        }
    }
}
