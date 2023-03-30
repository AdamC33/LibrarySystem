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
        private Member _thisMember;
        private int _index;

        public FineAdd(Member thisMember, int index = -1, string amount = null, string reason = null)
        {
            InitializeComponent();
            _thisMember = thisMember;
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
                controller.UpdateFee(_thisMember._cardNumber, amount, txtReason.Text, _index);
            }
            else
            {
                controller.AddFee(_thisMember._cardNumber, amount, txtReason.Text);
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

        private void txtAmount_TextChanged(object sender, TextChangedEventArgs e)
        {
            //This code only allows numbers to be entered into the amount textbox
            int initialSelectionStart = ((TextBox)sender).SelectionStart;
            string txtAmountString = ((TextBox)sender).Text;
            foreach (char c in ((TextBox)sender).Text)
            {
                if (!Char.IsDigit(c))
                {
                    //If the character isn't a digit, it gets removed from the string
                    txtAmountString = txtAmountString.Remove(txtAmountString.IndexOf(c), 1);
                    initialSelectionStart--;
                }
            }
            ((TextBox)sender).Text = txtAmountString;
            ((TextBox)sender).SelectionStart = initialSelectionStart;

            checkTextBoxes();
        }

        private void txtAmountPence_TextChanged(object sender, TextChangedEventArgs e)
        {
            //This code only allows numbers to be entered into the pence amount textbox
            int initialSelectionStart = ((TextBox)sender).SelectionStart;
            string txtAmountString = ((TextBox)sender).Text;
            foreach (char c in ((TextBox)sender).Text)
            {
                if (!Char.IsDigit(c) || txtAmountString.Length > 2)
                {
                    //If the character isn't a digit, it gets removed from the string
                    //If the length of the string is 2, nothing can be added to it
                    //This is because there is 100 pence in a pound. Only 2 digits are needed to store pence.
                    //The rest can be represented by the pound part of the £x.xx format
                    txtAmountString = txtAmountString.Remove(initialSelectionStart - 1, 1);
                    initialSelectionStart--;
                }
            }
            ((TextBox)sender).Text = txtAmountString;
            ((TextBox)sender).SelectionStart = initialSelectionStart;

            checkTextBoxes();
        }
    }
}
