using LibrarySystem.xaml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace LibrarySystem
{
    /// <summary>
    /// Interaction logic for AddMember.xaml
    /// </summary>
    public partial class AddMember : Window
    {
        private bool _modifying = true;
        private string _oldCardNumber;
        private string _oldPassword = null;
        private string _password;

        public AddMember(string cardNumber = null, string name = null, string phoneNumber = null, string email = null, string password = null, bool editingFromMemberAccount = false)
        {
            InitializeComponent();
            if (cardNumber == null)
            {
                _modifying = false;
                btnConfirmAndChangePassword.Content = "Confirm";
                btnConfirmWithoutChangingPassword.Visibility = Visibility.Collapsed;
                Height = 260;
            }
            else
            {
                _oldCardNumber = cardNumber;
                _oldPassword = password;
                txtCardNumber.Text = cardNumber;
                txtCardNumber.IsEnabled = false;
                txtName.Text = name;
                txtPhoneNumber.Text = phoneNumber.Remove(3, 1).Remove(6, 1); //Takes away the hyphens from the phone number string
                txtEmail.Text = email;
                if (editingFromMemberAccount)
                {
                    btnConfirmWithoutChangingPassword.Content = "Confirm";
                    btnConfirmAndChangePassword.Visibility = Visibility.Collapsed;
                    Height = 260;
                }
            }
        }

        private void btnConfirmAndChangePassword_Click(object sender, RoutedEventArgs e)
        {
            btnConfirmAndChangePassword.IsEnabled = false;
            XmlController controller = new XmlController();
            if (controller.GetMemberName(txtCardNumber.Text) != null && txtCardNumber.Text != _oldCardNumber)
            {
                MessageBox.Show("Cannot save member - conflicting card number!", "Save Member");
            }
            else
            {
                EnterPassword enterPassword = new EnterPassword(_oldPassword);
                enterPassword.Owner = Application.Current.MainWindow; //The password string can be passed from the main window to this page
                if (enterPassword.ShowDialog() == true)
                {
                    _password = ((MainWindow)Application.Current.MainWindow)._temporaryPassthroughString;
                    ((MainWindow)Application.Current.MainWindow)._temporaryPassthroughString = null;
                    Member newMember = new Member(
                    txtCardNumber.Text,
                    txtName.Text,
                    txtPhoneNumber.Text.Insert(3, "-").Insert(7, "-"), //Adds the hypens back to the phone number string
                    txtEmail.Text,
                    false,
                    null,
                    null,
                    null,
                    _password);

                    if (_modifying) { controller.UpdateMember(_oldCardNumber, newMember); }
                    else { controller.AddMember(newMember); }
                    this.Close();
                }
            }
            btnConfirmAndChangePassword.IsEnabled = true;
        }

        private void btnConfirmWithoutChangingPassword_Click(object sender, RoutedEventArgs e)
        {
            XmlController controller = new XmlController();
            if (controller.GetMemberName(txtCardNumber.Text) != null && txtCardNumber.Text != _oldCardNumber)
            {
                MessageBox.Show("Cannot save member - conflicting card number!", "Save Member");
            }
            else
            {
                Member newMember = new Member(
                txtCardNumber.Text,
                txtName.Text,
                txtPhoneNumber.Text.Insert(3, "-").Insert(7, "-"), //Adds the hypens back to the phone number string
                txtEmail.Text,
                false,
                null,
                null,
                null,
                _oldPassword);

                controller.UpdateMember(_oldCardNumber, newMember);
                this.Close();
            }
        }

        private void checkTextBoxes(object sender = null, TextChangedEventArgs e = null)
        {
            //Enables confirm button to be clicked if there is some text in all boxes
            if (txtCardNumber.Text.Length == 9
                && txtName.Text.Length > 0
                && txtPhoneNumber.Text.Length == 10
                && txtEmail.Text.Length > 0)
            {
                btnConfirmAndChangePassword.IsEnabled = true;
                btnConfirmWithoutChangingPassword.IsEnabled = true;
            }
            else
            {
                btnConfirmAndChangePassword.IsEnabled = false;
                btnConfirmWithoutChangingPassword.IsEnabled = false;
            }
        }

        private void txtNumber_TextChanged(object sender, TextChangedEventArgs e)
        {
            //This code only allows numbers to be entered into the textboxes where only numbers are to be entered
            int initialSelectionStart = ((TextBox)sender).SelectionStart;
            string txtNumberString = ((TextBox)sender).Text;
            foreach (char c in ((TextBox)sender).Text)
            {
                if (!Char.IsDigit(c))
                {
                    //If the character isn't a digit, it gets removed from the string
                    txtNumberString = txtNumberString.Remove(txtNumberString.IndexOf(c), 1);
                    initialSelectionStart--;
                }
            }
            ((TextBox)sender).Text = txtNumberString;
            ((TextBox)sender).SelectionStart = initialSelectionStart;

            checkTextBoxes();
        }
    }
}
