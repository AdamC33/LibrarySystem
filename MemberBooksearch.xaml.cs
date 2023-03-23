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

        public MemberBooksearch(string searchQuery, string searchBy, Member currentUser)
        {
            InitializeComponent();
            _currentUser = currentUser;
            XmlController controller = new XmlController();
            listSearchResults.ItemsSource = controller.GetLibrary(searchQuery, searchBy);
        }

        private void pageLoaded(object sender, RoutedEventArgs e)
        {
            NavigationService.RemoveBackEntry();
        }

        private void listSearchResults_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            NavigationService.Navigate(new MemberBookDetails((Book)listSearchResults.SelectedItem, _currentUser));
        }
    }
}
