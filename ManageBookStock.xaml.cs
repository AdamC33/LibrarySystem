using System;
using System.Collections.Generic;
using System.Globalization; //Needed for the date format to be correct (date-month-year as opposed to month-date-year)
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
            txtStock.Text = String.Format("{0} / {1}", _thisBook.currentStock, _thisBook._totalStock);
            XmlController controller = new XmlController();

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
                }) ;
            }
            listChecked.ItemsSource = checkBookList;
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
    }
}
