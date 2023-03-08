using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace LibrarySystem
{
    public partial class Login : Page
    {
        public Login()
        {
            InitializeComponent();
        }

        private void TextChanged(object sender, RoutedEventArgs e)
        {
            //This code only allows numbers to be entered into the username textbox
            int initialSelectionStart = txtCardNo.SelectionStart;
            string txtCardNoString = txtCardNo.Text;
            foreach (char c in txtCardNo.Text)
            {
                if (!Char.IsDigit(c))
                {
                    //If the character isn't a digit, it gets removed from the string
                    txtCardNoString = txtCardNoString.Remove(txtCardNoString.IndexOf(c), 1);
                    initialSelectionStart--;
                }
            }
            txtCardNo.Text = txtCardNoString;
            txtCardNo.SelectionStart = initialSelectionStart;
            //Enables login button to be clicked if the lengths are correct. Every card number is 9 digits long.
            if (txtCardNo.Text.Length == 9 && password.SecurePassword.Length > 0)
            {
                btnLogin.IsEnabled = true;
            }
            else
            {
                btnLogin.IsEnabled = false;
            }
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("MemberMainpage.xaml", UriKind.Relative));
            //https://learn.microsoft.com/en-us/dotnet/desktop/wpf/app-development/how-to-navigate-to-a-page?view=netframeworkdesktop-4.8
        }
    }
}
