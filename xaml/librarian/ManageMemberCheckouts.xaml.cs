using System;
using System.Collections.Generic;
using System.Globalization; //Needed for the date format to be correct (date-month-year as opposed to month-date-year). Same as ManageBookStock
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
    /// Interaction logic for ManageMemberCheckouts.xaml
    /// </summary>
    public partial class ManageMemberCheckouts : Page
    {
        private Member _thisMember;

        public ManageMemberCheckouts(Member thisMember)
        {
            InitializeComponent();
            _thisMember = thisMember;
            lblCardNumber.Content = String.Format("Member {0}'s Library:", _thisMember._cardNumber);
            UpdateDisplay();
        }

        private void UpdateDisplay()
        {
            XmlController controller = new XmlController();

            List<CheckBookDisplay>[] memberLibraryList = CheckBookDisplay.GetMemberLibraryDisplay(_thisMember);

            listChecked.ItemsSource = memberLibraryList[0].OrderBy(o => o.getDueDate);
            listQueued.ItemsSource = memberLibraryList[1];
        }

        private void btnNotify_Click(object sender, RoutedEventArgs e)
        {
            CheckBookDisplay thisBook = (CheckBookDisplay)((Button)sender).DataContext;
            XmlController controller = new XmlController();
            if (controller.NotifyMember(_thisMember._cardNumber, String.Format("{0} ({1}) is past its due date and you have still not returned it - please return it!", thisBook.title, thisBook.ISBN)))
            {
                MessageBox.Show("Successfully notified the member!", "Notify Member");
            }
            else
            {
                MessageBox.Show("Could not notify the member!", "Notify Member");
            }
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}
