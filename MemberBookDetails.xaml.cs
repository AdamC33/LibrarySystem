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
        }

        private void btnBookCheck_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedBook.getDueDate(_currentUser._cardNumber) == DateTimeOffset.MinValue) //This is run if the book cannot be found.
            //The getDueDate function returns with the minimum datetimeoffset value if it cannot find anything in the book's checkout list.
            {
                //Code for checking out book goes here...
                MessageBox.Show("Successfully checked out book!");
            }
            else if (_selectedBook.getDueDate(_currentUser._cardNumber) == DateTimeOffset.FromUnixTimeSeconds(0))
            {
                MessageBox.Show("You are already queued for this book!");
            }
            else
            {
                MessageBox.Show("Book is already checked out!");
            }
        }
    }
}
