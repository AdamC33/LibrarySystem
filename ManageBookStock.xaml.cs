using System;
using System.Collections.Generic;
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
            public DateTimeOffset dueDate { get; set; }
        }
        public ManageBookStock(Book thisBook)
        {
            InitializeComponent();
            _thisBook = thisBook;
            txtStock.Text = String.Format("{0} / {1}", _thisBook.currentStock, _thisBook._totalStock);
            XmlController controller = new XmlController();

            List<checkBookDisplay> checkBookList = new List<checkBookDisplay>();
            for (int i = 0; i < _thisBook.checkoutListMinusQueueLength; i++)
            {
                checkBookList.Add(new checkBookDisplay
                {
                    cardNumber = _thisBook.getCardNumber(i),
                    name = controller.GetMemberName(_thisBook.getCardNumber(i)),
                    dueDate = _thisBook.getDueDate(_thisBook.getCardNumber(i))
                });
            }
            listChecked.ItemsSource = checkBookList;
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}
