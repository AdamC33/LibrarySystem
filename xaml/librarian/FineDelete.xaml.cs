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
    /// Interaction logic for FineDelete.xaml
    /// </summary>
    public partial class FineDelete : Window
    {
        private string _cardNumber;
        private int _index;
        private bool _hasInitialised = false;
        private int _fineAmount;

        public FineDelete(string cardNumber, int index)
        {
            InitializeComponent();
            _cardNumber = cardNumber;
            _index = index;
            _hasInitialised = true;
        }

        private void checkRadioButtons(object sender, RoutedEventArgs e)
        {
            if (radPaid.IsChecked == true && _hasInitialised)
            {
                txtOther.IsEnabled = false;
                btnConfirm.IsEnabled = true;
            }
            else if (_hasInitialised && (radOther.IsChecked == true && txtOther.Text.Length == 0))
            {
                txtOther.IsEnabled = true;
                btnConfirm.IsEnabled = false;
            }
            else if (_hasInitialised && (radOther.IsChecked == true && txtOther.Text.Length > 0))
            {
                txtOther.IsEnabled = true;
                btnConfirm.IsEnabled = true;
            }
        }

        private void txtOther_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (txtOther.Text.Length > 0)
            {
                btnConfirm.IsEnabled = true;
            }
            else
            {
                btnConfirm.IsEnabled = false;
            }
        }

        private void btnConfirm_Click(object sender, RoutedEventArgs e)
        {
            //TODO: Add to log file w/ explanation for why fine was removed
            XmlController controller = new XmlController();
            string reason = txtOther.Text;
            if (radPaid.IsChecked == true)
            {
                reason = "Fine has been paid.";
            }
            controller.DeleteFee(_cardNumber, _index);
            controller.NotifyMember(_cardNumber, String.Format("A fine of {0} has been removed from your account with the reason: {1}", _fineAmount, reason));
            this.Close();
        }
    }
}
