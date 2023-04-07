using LibrarySystem.xaml;
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
            if (dgMembers.SelectedItem != null)
            {
                btnMod.IsEnabled = true;
                btnRem.IsEnabled = true;
                btnFines.IsEnabled = true;
                btnLibrary.IsEnabled = true;
            }
            else
            {
                btnMod.IsEnabled = false;
                btnRem.IsEnabled = false;
                btnFines.IsEnabled = false;
                btnLibrary.IsEnabled = false;
            }
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private void btnLibrary_Click(object sender, RoutedEventArgs e)
        {
            DataRowView row = dgMembers.SelectedItem as DataRowView;
            XmlController controller = new XmlController();
            Member thisMember = controller.GetMember(row.Row.ItemArray[0].ToString(), row.Row.ItemArray[1].ToString());
            NavigationService.Navigate(new ManageMemberCheckouts(thisMember));

            dataSet.Reset();
            dataSet.ReadXml(@membersPath);
            dataSet.Tables[0].Rows[0].Delete();
            dgMembers.ItemsSource = dataSet.Tables[0].DefaultView;
        }

        private void btnFines_Click(object sender, RoutedEventArgs e)
        {
            DataRowView row = dgMembers.SelectedItem as DataRowView;
            XmlController controller = new XmlController();
            Member thisMember = controller.GetMember(row.Row.ItemArray[0].ToString(), row.Row.ItemArray[1].ToString());
            NavigationService.Navigate(new ManageMemberFines(thisMember));

            dataSet.Reset();
            dataSet.ReadXml(@membersPath);
            dataSet.Tables[0].Rows[0].Delete();
            dgMembers.ItemsSource = dataSet.Tables[0].DefaultView;
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            AddMember addMember = new AddMember();
            addMember.ShowDialog(); //ShowDialog is used instead of Show as it pauses the main window.
            //Any code after this is will be run after the addMember window has closed (either because the user has clicked "Confirm" or the close button in the top right)
            dataSet.Reset();
            dataSet.ReadXml(@membersPath);
            dataSet.Tables[0].Rows[0].Delete();
            dgMembers.ItemsSource = dataSet.Tables[0].DefaultView;
        }

        private void btnMod_Click(object sender, RoutedEventArgs e)
        {
            DataRowView row = dgMembers.SelectedItem as DataRowView;
            AddMember modMember = new AddMember(
            row.Row.ItemArray[0].ToString(),
            row.Row.ItemArray[2].ToString(),
            row.Row.ItemArray[3].ToString(),
            row.Row.ItemArray[4].ToString(),
            row.Row.ItemArray[5].ToString());
            modMember.ShowDialog();

            dataSet.Reset();
            dataSet.ReadXml(@membersPath);
            dataSet.Tables[0].Rows[0].Delete();
            dgMembers.ItemsSource = dataSet.Tables[0].DefaultView;
        }

        private void btnRem_Click(object sender, RoutedEventArgs e)
        {
            XmlController controller = new XmlController();
            DataRowView row = dgMembers.SelectedItem as DataRowView;
            MessageBoxResult yesOrNo = MessageBox.Show("Are you sure you want to delete this member?", "Delete Member", MessageBoxButton.YesNo);
            if (yesOrNo == MessageBoxResult.Yes)
            {
                controller.DeleteMember(row.Row.ItemArray[0].ToString());
            }

            dataSet.Reset();
            dataSet.ReadXml(@membersPath);
            dataSet.Tables[0].Rows[0].Delete();
            dgMembers.ItemsSource = dataSet.Tables[0].DefaultView;
        }
    }
}
