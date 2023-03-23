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
    public partial class MemberHomepage : Page
    {
        private Member _currentUser;
        private Book _book1;
        private Book _book2;
        private Book _book3;
        private Book _book4;
        public MemberHomepage(Member currentUser)
        {
            InitializeComponent();
            _currentUser = currentUser;
            XmlController controller = new XmlController();
            List<Book> library = controller.GetLibrary();

            //Sets up the new books on the homepage
            //The "newest books" in this case are the ones that are furthest down the XML file
            frameBook1.Content = new BookFrameHomepage();
            frameBook2.Content = new BookFrameHomepage();
            frameBook3.Content = new BookFrameHomepage();
            frameBook4.Content = new BookFrameHomepage();
            try
            {
                _book1 = library[library.Count() - 1];
                frameBook1.Content = new BookFrameHomepage(_book1);
                btnBook1.IsEnabled = true;
                _book2 = library[library.Count() - 2];
                frameBook2.Content = new BookFrameHomepage(_book2);
                btnBook2.IsEnabled = true;
                _book3 = library[library.Count() - 3];
                frameBook3.Content = new BookFrameHomepage(_book3);
                btnBook3.IsEnabled = true;
                _book4 = library[library.Count() - 4];
                frameBook4.Content = new BookFrameHomepage(_book4);
                btnBook4.IsEnabled = true;
            }
            catch (ArgumentOutOfRangeException)
            { 
                //If there are less than 4 books in the XML file, this acts as a failsafe so that it displays "TBA" in all of the fields instead of the program crashing
            }
        }

        private void btnBook1_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new MemberBookDetails(_book1, _currentUser));
        }

        private void btnBook2_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new MemberBookDetails(_book2, _currentUser));
        }

        private void btnBook3_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new MemberBookDetails(_book3, _currentUser));
        }

        private void btnBook4_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new MemberBookDetails(_book4, _currentUser));
        }
    }
}
