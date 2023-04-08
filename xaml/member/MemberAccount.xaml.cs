using LibrarySystem.xaml;
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
        private string _password;

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

        private void btnEditDetails_Click(object sender, RoutedEventArgs e)
        {
            AddMember modMember = new AddMember(
            _currentUser._cardNumber,
            _currentUser._name,
            _currentUser._phoneNumber,
            _currentUser._email,
            _currentUser.getPassword,
            true
            );
            modMember.ShowDialog();

            UpdateDisplay();
        }

        private void btnChangePassword_Click(object sender, RoutedEventArgs e)
        {
            EnterPassword enterPassword = new EnterPassword(_currentUser.getPassword);
            enterPassword.Owner = Application.Current.MainWindow;
            if (enterPassword.ShowDialog() == true)
            {
                XmlController controller = new XmlController();

                _password = ((MainWindow)Application.Current.MainWindow)._temporaryPassthroughString;
                ((MainWindow)Application.Current.MainWindow)._temporaryPassthroughString = null;

                controller.UpdateMemberPassword(_currentUser._cardNumber, _currentUser.getPassword, _password);
            }
        }

        private void btnReturn_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to return this book?", "Return Book", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                XmlController controller = new XmlController();
                Book thisBook = controller.GetLibrary(((CheckBookDisplay)((Button)sender).DataContext).ISBN, "isbn")[0];
                int daysLate = thisBook.returnBook(_currentUser._cardNumber);
                string messageBoxText = "Successfully returned book!";
                if (daysLate > 0)
                {
                    //If a book is late, the amount they are charged is (days late * 25) in pence.
                    controller.AddFee(
                        _currentUser._cardNumber,
                        MoneyFormat.AddSignAndDecimalPenceOnly(daysLate * 25),
                        String.Format("Late return of book (ISBN {0})", thisBook._ISBN));
                    messageBoxText = String.Format("Returned late book successfully, a fine of {0} has been added to your account.", MoneyFormat.AddSignAndDecimalPenceOnly(daysLate * 25));
                }
                controller.UpdateBookCheckout(thisBook);
                MessageBox.Show(messageBoxText, "Return Book");
            }
            UpdateDisplay();
        }

        private void btnRenew_Click(object sender, RoutedEventArgs e)
        {
            XmlController controller = new XmlController();
            Book thisBook = controller.GetLibrary(((CheckBookDisplay)((Button)sender).DataContext).ISBN, "isbn")[0];
            if (thisBook.getRenewedStatus(_currentUser._cardNumber) == true ) { MessageBox.Show("Cannot renew books that have already previously been renewed!", "Renew Book"); }
            else if (MessageBox.Show("Are you sure you want to renew this book?", "Renew Book", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                int daysLate = thisBook.renewBook(_currentUser._cardNumber);
                string messageBoxText = "Successfully renewed book!";
                if (daysLate > 0)
                {
                    //If a book is late, the amount they are charged is (days late * 25) in pence.
                    controller.AddFee(
                        _currentUser._cardNumber,
                        MoneyFormat.AddSignAndDecimalPenceOnly(daysLate * 25),
                        String.Format("Late renewal of book (ISBN {0})", thisBook._ISBN));
                    messageBoxText = String.Format("Renewed late book successfully, a fine of {0} has been added to your account.", MoneyFormat.AddSignAndDecimalPenceOnly(daysLate * 25));
                }
                controller.UpdateBookCheckout(thisBook);
                MessageBox.Show(messageBoxText, "Renew Book");
            }
            UpdateDisplay();
        }
    }
}
