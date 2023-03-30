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

        public AddMember(string cardNumber = null, string name = null, string phoneNumber = null, string email = null)
        {
            InitializeComponent();
            if (cardNumber == null)
            {
                _modifying = false;
            }
            else
            {
                _oldCardNumber = cardNumber;
                txtCardNumber.Text = cardNumber;
                txtCardNumber.IsEnabled = false;
                txtName.Text = name;
                txtPhoneNumber.Text = phoneNumber.Remove(3, 1).Remove(6, 1); //Takes away the hyphens from the phone number string
                txtEmail.Text = email;
            }
        }

        private void btnConfirm_Click(object sender, RoutedEventArgs e)
        {
            btnConfirm.IsEnabled = false;
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
                txtEmail.Text );

                if (_modifying) { controller.UpdateMember(_oldCardNumber, newMember); }
                else { controller.AddMember(newMember); }
                this.Close();
            }
            btnConfirm.IsEnabled = true;
        }

        private void checkTextBoxes(object sender = null, TextChangedEventArgs e = null)
        {
            //Enables confirm button to be clicked if there is some text in all boxes
            if (txtCardNumber.Text.Length == 9
                && txtName.Text.Length > 0
                && txtPhoneNumber.Text.Length == 10
                && txtEmail.Text.Length > 0)
            {
                btnConfirm.IsEnabled = true;
            }
            else
            {
                btnConfirm.IsEnabled = false;
            }
        }

        private void txtCardNumber_TextChanged(object sender, TextChangedEventArgs e)
        {
            //This code only allows numbers to be entered into the card number textbox
            int initialSelectionStart = txtCardNumber.SelectionStart;
            string txtCardNumberString = txtCardNumber.Text;
            foreach (char c in txtCardNumber.Text)
            {
                if (!Char.IsDigit(c))
                {
                    //If the character isn't a digit, it gets removed from the string
                    txtCardNumberString = txtCardNumberString.Remove(txtCardNumberString.IndexOf(c), 1);
                    initialSelectionStart--;
                }
            }
            txtCardNumber.Text = txtCardNumberString;
            txtCardNumber.SelectionStart = initialSelectionStart;

            checkTextBoxes();
        }

        private void txtPhoneNumber_TextChanged(object sender, TextChangedEventArgs e)
        {
            //This code only allows numbers to be entered into the phone number textbox
            int initialSelectionStart = txtPhoneNumber.SelectionStart;
            string txtPhoneNumberString = txtPhoneNumber.Text;
            foreach (char c in txtPhoneNumber.Text)
            {
                if (!Char.IsDigit(c))
                {
                    //If the character isn't a digit, it gets removed from the string
                    txtPhoneNumberString = txtPhoneNumberString.Remove(txtPhoneNumberString.IndexOf(c), 1);
                    initialSelectionStart--;
                }
            }
            txtPhoneNumber.Text = txtPhoneNumberString;
            txtPhoneNumber.SelectionStart = initialSelectionStart;

            checkTextBoxes();
        }
    }
}
