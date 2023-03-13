using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
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
    /// Interaction logic for MemberBooksearch.xaml
    /// </summary>
    public partial class MemberBooksearch : Page
    {
        private Member _currentUser;

        public MemberBooksearch(string searchQuery, byte searchBy, Member currentUser)
        {
            int[] somearray = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30 };
            InitializeComponent();
            _currentUser = currentUser;
            listSearchResults.ItemsSource = somearray;
        }

        private void btnHome_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.RemoveBackEntry();
            NavigationService.Navigate(new MemberHomepage());
        }
    }
}
