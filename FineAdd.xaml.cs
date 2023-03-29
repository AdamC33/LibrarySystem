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
        public bool _modifying = true;

        public FineAdd(string amount = null, string reason = null)
        {
            InitializeComponent();
            if (amount == null)
            {
                _modifying = false;
            }
            else
            {
                txtAmount.Text = amount.ToString().Remove(0, 1).Remove((amount.Length - 4), 1); //Takes away the pound sign and decimal point
                txtReason.Text = reason;
            }
        }

        private void checkTextBoxes(object sender = null, TextChangedEventArgs e = null)
        {

        }

        private void txtAmount_TextChanged(object sender, TextChangedEventArgs e)
        {
            //This code only allows numbers to be entered into the amount textbox
            /*int initialSelectionStart = txtAmount.SelectionStart;
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

            checkTextBoxes();*/
        }

        private void btnConfirm_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
