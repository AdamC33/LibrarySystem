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
        public BookFrameHomepage(string title = "TBA", string author = "TBA", string year = "TBA", string publisher = "TBA", string category = "TBA")
        {
            InitializeComponent();
            txtBookTitle.Text = title;
            txtBookAuthor.Text = author;
            txtBookYear.Text = year;
            txtBookPublisher.Text = publisher;
            txtBookCategory.Text = category;
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
