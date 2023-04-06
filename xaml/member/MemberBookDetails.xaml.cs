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
    /// <summary>
    /// Interaction logic for MemberBookDetails.xaml
    /// </summary>
    public partial class MemberBookDetails : Page
    {
        private Book _selectedBook;
        private Member _currentUser;

        public MemberBookDetails(Book selectedBook, Member currentUser)
        {
            InitializeComponent();
            _selectedBook = selectedBook;
            _currentUser = currentUser;
            txtBookTitle.Text = selectedBook._title;
            txtBookAuthor.Text = selectedBook._author;
            txtBookYear.Text = selectedBook._year;
            txtBookPublisher.Text = selectedBook._publisher;
            txtBookCategory.Text = selectedBook._category;
            txtBookISBN.Text = selectedBook._ISBN;
            UpdateDisplay();
        }

        private void UpdateDisplay()
        {
            XmlController controller = new XmlController();
            _selectedBook = controller.GetLibrary(_selectedBook._ISBN, "isbn")[0];
            txtBookStock.Text = Convert.ToString(_selectedBook.currentStock);
        }

        private void pageLoaded(object sender, RoutedEventArgs e)
        {
            NavigationService.RemoveBackEntry();
        }

        private void btnBookCheck_Click(object sender, RoutedEventArgs e)
        {
            int bookCheckoutValue = _selectedBook.checkoutBook(_currentUser._cardNumber);
            XmlController controller = new XmlController();
            controller.UpdateBookCheckout(_selectedBook);
            switch (bookCheckoutValue)
            {
                case 0:
                    MessageBox.Show("You already have the book checked out!", "Check Book");
                    break;
                case 1:
                    MessageBox.Show("You are already queued for this book!", "Check Book");
                    break;
                case 2:
                    MessageBox.Show("Successfully added to queue! You will automatically check the book out once enough stock is available.", "Check Book");
                    break;
                case 3:
                    MessageBox.Show("Successfully checked out book!", "Check Book");
                    break;
                default:
                    MessageBox.Show("You should not be seeing this message box! If you do, please contact your system administrator!", "Check Book");
                    break;
            }
            UpdateDisplay();
        }
    }
}
