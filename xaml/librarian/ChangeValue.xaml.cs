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
    /// Interaction logic for ChangeValue.xaml
    /// </summary>
    public partial class ChangeValue : Window
    {
        private Book _thisBook;

        public ChangeValue(Book thisBook)
        {
            InitializeComponent();
            _thisBook = thisBook;
        }

        private void txtNewValue_TextChanged(object sender, TextChangedEventArgs e)
        {
            //This code only allows numbers to be entered into the value textbox
            int initialSelectionStart = txtNewValue.SelectionStart;
            string txtValueString = txtNewValue.Text;
            foreach (char c in txtNewValue.Text)
            {
                if (!Char.IsDigit(c) || txtValueString.Length > 9)
                {
                    //If the character isn't a digit, it gets removed from the string
                    //If the length of the string is greater than 9, nothing can be added to it
                    //This is to avoid exceeding the maximum unsigned integer value, which is 4,294,967,295
                    //With this string limit imposed, the maximum value that can be entered is 999,999,999
                    txtValueString = txtValueString.Remove(initialSelectionStart - 1, 1);
                    initialSelectionStart--;
                }
            }
            txtNewValue.Text = txtValueString;
            txtNewValue.SelectionStart = initialSelectionStart;

            //Enables confirm button to be clicked if there is some text in the textbox
            if (txtNewValue.Text.Length > 0)
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
            if (_thisBook.modifyStock(Convert.ToUInt32(txtNewValue.Text)))
            {
                XmlController controller = new XmlController();
                controller.UpdateBookStock(_thisBook._ISBN, Convert.ToUInt32(txtNewValue.Text));
                DialogResult = true;
                this.Close();
            }
            else
            {
                MessageBox.Show("Inputted stock level is too low! It must be higher than or equal to the number of people who have checked the book out.", "Change Book Stock");
            }
        }
    }
}
