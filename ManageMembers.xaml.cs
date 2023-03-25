using System;
using System.Collections.Generic;
using System.Data;
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
    /// Interaction logic for ManageMembers.xaml
    /// </summary>
    public partial class ManageMembers : Page
    {
        private string membersPath = "Members.xml";
        private DataSet dataSet = new DataSet();

        public ManageMembers()
        {
            InitializeComponent();
            dataSet.ReadXml(@membersPath);
            dataSet.Tables[0].Rows[0].Delete(); //Excludes the librarian's account from the display. Their account is important and changing details on it would break the system.
            dgMembers.ItemsSource = dataSet.Tables[0].DefaultView;
        }

        private void dgMembers_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private void btnFines_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnMod_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnRem_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
