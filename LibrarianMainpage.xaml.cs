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
    /// Interaction logic for LibrarianMainpage.xaml
    /// </summary>
    public partial class LibrarianMainpage : Page
    {
        public LibrarianMainpage()
        {
            InitializeComponent();
        }

        private void btnManageBooks_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new ManageBooks());
        }

        private void btnLogout_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Login());
        }
    }
}
