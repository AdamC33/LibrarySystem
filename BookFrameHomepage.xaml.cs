using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
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
    /// Interaction logic for BookFrameHomepage.xaml
    /// </summary>
    public partial class BookFrameHomepage : Page
    {
        private Book _thisBook;
        public BookFrameHomepage(Book thisBook)
        {
            InitializeComponent();
            _thisBook = thisBook;
            txtBookTitle.Text = _thisBook._title;
            txtBookAuthor.Text = _thisBook._author;
            txtBookYear.Text = _thisBook._year;
            txtBookPublisher.Text = _thisBook._publisher;
            txtBookCategory.Text = _thisBook._category;
        }

        public BookFrameHomepage()
        {
            InitializeComponent();
            txtBookTitle.Text = "TBA";
            txtBookAuthor.Text = "TBA";
            txtBookYear.Text = "TBA";
            txtBookPublisher.Text = "TBA";
            txtBookCategory.Text = "TBA";
        }
    }
}
