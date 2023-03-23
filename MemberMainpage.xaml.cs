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
    /// Interaction logic for MemberMainpage.xaml
    /// </summary>
    public partial class MemberMainpage : Page
    {
        public static bool homepageFirstLoaded; //Used so when the homepage is loaded in for the first time, it doesn't try to remove the back entry (the login screen).

        private Member _currentUser;

        public MemberMainpage(Member currentUser)
        {
            InitializeComponent();
            homepageFirstLoaded = false;
            _currentUser = currentUser;
            txtWelcome.Text = String.Format("Welcome, {0}!", currentUser._name);
            frameMember.Content = new MemberHomepage(_currentUser);
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            string searchBy;
            if (radISBN.IsChecked == true)
            {
                searchBy = "isbn";
            }
            else if (radAuthor.IsChecked == true)
            {
                searchBy = "author";
            }
            else if (radYear.IsChecked == true)
            {
                searchBy = "year";
            }
            else if (radPublisher.IsChecked == true)
            {
                searchBy = "publisher";
            }
            else if (radCategory.IsChecked == true)
            {
                searchBy = "category";
            }
            else
            {
                searchBy = "title";
            }
            frameMember.Content = new MemberBooksearch(txtSearch.Text, searchBy, _currentUser);
        }

        private void btnHome_Click(object sender, RoutedEventArgs e)
        {
            frameMember.Content = new MemberHomepage(_currentUser);
        }

        private void btnLogout_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Login());
        }
    }
}
