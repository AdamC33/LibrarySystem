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

        public Member _currentUser;

        public MemberMainpage(Member currentUser)
        {
            InitializeComponent();
            homepageFirstLoaded = false;
            UpdateDisplay(currentUser);
            frameMember.Content = new MemberHomepage(_currentUser);
        }

        public void UpdateDisplay(Member currentUser)
        {
            XmlController controller = new XmlController();
            if (((MainWindow)Application.Current.MainWindow)._temporaryPassthroughString != null)
            {
                _currentUser = controller.GetMember(currentUser._cardNumber, ((MainWindow)Application.Current.MainWindow)._temporaryPassthroughString);
                ((MainWindow)Application.Current.MainWindow)._temporaryPassthroughString = null;
            }
            else
            {
                _currentUser = controller.GetMember(currentUser._cardNumber, currentUser.getPassword);
            }
            txtWelcome.Text = String.Format("Welcome, {0}!", _currentUser._name);
            if (_currentUser.getRequests.Count > 0)
            {
                imgUnread.Visibility = Visibility.Visible;
            }
            else
            {
                imgUnread.Visibility = Visibility.Collapsed;
            }
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
            UpdateDisplay(_currentUser);
        }

        private void btnHome_Click(object sender, RoutedEventArgs e)
        {
            frameMember.Content = new MemberHomepage(_currentUser);
            UpdateDisplay(_currentUser);
        }

        private void btnAccount_Click(object sender, RoutedEventArgs e)
        {
            frameMember.Content = new MemberAccount(_currentUser);
            UpdateDisplay(_currentUser);
        }

        private void btnNotifications_Click(object sender, RoutedEventArgs e)
        {
            frameMember.Content = new MemberRequests(_currentUser, this);
            UpdateDisplay(_currentUser);
        }

        private void btnLogout_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Login());
        }
    }
}
