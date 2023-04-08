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

namespace LibrarySystem.xaml
{
    /// <summary>
    /// Interaction logic for EnterPassword.xaml
    /// </summary>
    public partial class EnterPassword : Window
    {
        string _cardNumber;
        string _oldPassword;

        public EnterPassword(string cardNumber, string oldPassword = null)
        {
            InitializeComponent();
            _cardNumber = cardNumber;
            _oldPassword = oldPassword;
            if (oldPassword == null)
            {
                passwordOld.Visibility = Visibility.Collapsed;
                lblOldPassword.Visibility = Visibility.Collapsed;
                Height = 200;
            }
        }

        private void checkPasswordBoxes(object sender, RoutedEventArgs e)
        {
            if (passwordNew.Password == passwordNewConfirm.Password &&
                (_oldPassword == null || passwordOld.Password.Length > 0)
                && passwordNew.Password.Length > 0 && passwordNewConfirm.Password.Length > 0)
            {
                btnConfirm.IsEnabled = true;
            }
            else { btnConfirm.IsEnabled = false; }
        }

        private void btnConfirm_Click(object sender, RoutedEventArgs e)
        {
            XmlController controller = new XmlController();
            if (_oldPassword == null || _oldPassword == passwordOld.Password)
            {
                ((AddMember)Owner)._password = passwordNew.Password;
                this.DialogResult = true;
                this.Close();
            }
            else if (_oldPassword != passwordOld.Password)
            {
                lblWrongPassword.Content = "Incorrect old password!";
            }
        }
    }
}
