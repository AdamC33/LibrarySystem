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
        private Member _currentUser;

        public MemberMainpage(Member currentUser)
        {
            InitializeComponent();
            _currentUser = currentUser;
            frameMember.Content = new MemberHomepage();
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            byte searchBy;
            if (radISBN.IsChecked == true)
            {
                searchBy = 0b00000001;
            }
            else if (radAuthor.IsChecked == true)
            {
                searchBy = 0b00000010;
            }
            else if (radYear.IsChecked == true)
            {
                searchBy = 0b00000011;
            }
            else if (radPublisher.IsChecked == true)
            {
                searchBy = 0b00000100;
            }
            else if (radCategory.IsChecked == true)
            {
                searchBy = 0b00000101;
            }
            else
            {
                searchBy = 0b00000000;
            }
            NavigationService.RemoveBackEntry();
            frameMember.Content = new MemberBooksearch(txtSearch.Text, searchBy, _currentUser);
        }
    }
}
