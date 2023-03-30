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
                txtAmount.Text = amount.Remove(0, 1).Remove((amount.Length - 4), 1); //Takes away the pound sign and decimal point
                txtReason.Text = reason;
            }
        }

        private void btnConfirm_Click(object sender, RoutedEventArgs e)
        {
            btnConfirm.IsEnabled = false;
            XmlController controller = new XmlController();
            string amount = txtAmount.Text.Insert(0, "£").Insert(txtAmount.Text.Length - 1, ".");
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
            if (txtAmount.Text.Length > 0)
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
            int initialSelectionStart = txtAmount.SelectionStart;
            string txtAmountString = txtAmount.Text;
            foreach (char c in txtAmount.Text)
            {
                if (!Char.IsDigit(c))
                {
                    //If the character isn't a digit, it gets removed from the string
                    txtAmountString = txtAmountString.Remove(txtAmountString.IndexOf(c), 1);
                    initialSelectionStart--;
                }
            }
            txtAmount.Text = txtAmountString;
            txtAmount.SelectionStart = initialSelectionStart;

            checkTextBoxes();
        }
    }
}
