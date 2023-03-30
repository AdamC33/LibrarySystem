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
    /// Interaction logic for FineAdd.xaml
    /// </summary>
    public partial class FineAdd : Window
    {
        private bool _modifying = true;
        private string _cardNumber;
        private int _index;

        public FineAdd(string cardNumber, int index = -1, string amount = null, string reason = null)
        {
            InitializeComponent();
            _cardNumber = cardNumber;
            _index = index;
            if (amount == null)
            {
                _modifying = false;
            }
            else
            {
                txtAmountPounds.Text = MoneyFormat.GetWithoutPence(amount);
                txtAmountPence.Text = MoneyFormat.GetOnlyPence(amount);
                txtReason.Text = reason;
            }
        }

        private void btnConfirm_Click(object sender, RoutedEventArgs e)
        {
            btnConfirm.IsEnabled = false;
            XmlController controller = new XmlController();
            string amount = MoneyFormat.AddSignAndDecimal(txtAmountPounds.Text, txtAmountPence.Text);
            if (_modifying)
            {
                controller.UpdateFee(_cardNumber, amount, txtReason.Text, _index);
            }
            else
            {
                controller.AddFee(_cardNumber, amount, txtReason.Text);
            }
            this.Close();
        }

        private void checkTextBoxes(object sender = null, TextChangedEventArgs e = null)
        {
            //Enables confirm button to be clicked if there is some text in the amount box
            if (txtAmountPounds.Text.Length > 0 && txtAmountPence.Text.Length == 2)
            {
                btnConfirm.IsEnabled = true;
            }
            else
            {
                btnConfirm.IsEnabled = false;
            }
        }

        private void txtAmount_TextChanged(object sender, TextChangedEventArgs e, int maxLength)
        {
            //This code only allows numbers to be entered into the amount textboxes
            int initialSelectionStart = ((TextBox)sender).SelectionStart;
            string txtAmountString = ((TextBox)sender).Text;
            foreach (char c in ((TextBox)sender).Text)
            {
                if (!Char.IsDigit(c) || txtAmountString.Length > maxLength)
                {
                    //If the character isn't a digit, it gets removed from the string
                    //If the length of the string is 'maxLength', nothing can be added to it
                    //The reason for this is explained in the functions which call this function
                    txtAmountString = txtAmountString.Remove(initialSelectionStart - 1, 1);
                    initialSelectionStart--;
                }
            }
            ((TextBox)sender).Text = txtAmountString;
            ((TextBox)sender).SelectionStart = initialSelectionStart;

            checkTextBoxes();
        }

        private void txtAmountPounds_TextChanged(object sender, TextChangedEventArgs e)
        {
            txtAmount_TextChanged(sender, e, 7);
            //7 is chosen as the max length to avoid exceeding the max unsigned integer value
            //(combining 7 from the pounds and 2 from the pence to get 9)
        }

        private void txtAmountPence_TextChanged(object sender, TextChangedEventArgs e)
        {
            txtAmount_TextChanged(sender, e, 2);
            //2 is chosen as only 2 digits are needed to represent pence
        }
    }
}
