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
    /// Interaction logic for MemberAccount.xaml
    /// </summary>
    public partial class MemberAccount : Page
    {
        private Member _currentUser;

        private class fineDisplay //Used for the items source binding
        {
            public string amount { get; set; }
            public string reason { get; set; }
        }

        public MemberAccount(Member currentUser)
        {
            InitializeComponent();
            _currentUser = currentUser;
            UpdateDisplay();
        }

        private void pageLoaded(object sender, RoutedEventArgs e)
        {
            NavigationService.RemoveBackEntry();
        }

        private void UpdateDisplay()
        {
            XmlController controller = new XmlController();
            _currentUser = controller.GetMember(_currentUser._cardNumber, _currentUser.getPassword);

            txtName.Text = _currentUser._name;
            txtPhoneNumber.Text = _currentUser._phoneNumber;
            txtEmail.Text = _currentUser._email;
            txtCardNumber.Text = _currentUser._cardNumber;

            List<CheckBookDisplay>[] memberLibraryList = CheckBookDisplay.GetMemberLibraryDisplay(_currentUser);
            List<fineDisplay> fineList = new List<fineDisplay>();

            for (int i = 0; i < _currentUser.feeListCount; i++)
            {
                fineList.Add(new fineDisplay
                {
                    amount = _currentUser.getFeeAmount(i),
                    reason = _currentUser.getFeeReason(i)
                });
            }

            listLibrary.ItemsSource = memberLibraryList[0].OrderBy(o => o.getDueDate);
            listQueued.ItemsSource = memberLibraryList[1];
            listFines.ItemsSource = fineList;
        }

        private void btnReturn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnRenew_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
