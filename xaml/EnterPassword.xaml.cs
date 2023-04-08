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
        string _oldPassword;

        //The new password is passed into the main window's temporary string - be sure to make it null again after this window has closed!

        public EnterPassword(string oldPassword = null)
        {
            InitializeComponent();
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
            if (_oldPassword == null || _oldPassword == passwordOld.Password)
            {
                ((MainWindow)Application.Current.MainWindow)._temporaryPassthroughString = passwordNew.Password;
                this.DialogResult = true;
                this.Close();
            }
            else if (_oldPassword != passwordOld.Password)
            {
                System.Windows.Media.Animation.DoubleAnimation animation = new System.Windows.Media.Animation.DoubleAnimation(1, 0, TimeSpan.FromSeconds(2));
                lblWrongPassword.BeginAnimation(Label.OpacityProperty, animation);
            }
        }
    }
}
