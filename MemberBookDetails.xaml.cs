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
    }
}
