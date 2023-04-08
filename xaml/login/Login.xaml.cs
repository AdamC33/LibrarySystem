using LibrarySystem.xaml;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
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
            //This code only allows numbers to be entered into the library card number textbox
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
            System.Windows.Media.Animation.DoubleAnimation animation = new System.Windows.Media.Animation.DoubleAnimation(1, 0, TimeSpan.FromSeconds(2));
            //This animation goes from a value of 1 to 0 within 2 seconds. It is used to fade out the wrong details label
            //It is also used in the EnterPassword.xaml.cs file

            XmlController controller = new XmlController();
            Member attemptMember = controller.GetMember(txtCardNo.Text, password.Password);
            if (attemptMember != null)
            {
                password.Password = null;
                if (txtCardNo.Text == "000000000")
                {
                    NavigationService.Navigate(new LibrarianMainpage());
                }
                else if (!attemptMember._activated)
                {
                    if (MessageBox.Show("Hello and welcome to the Library Bookings Co. library application! As you are a new member, please look through the user guide for the application and after activating your account, familiarise yourself with the application's interface. Once you are ready to activate your account, please click on the OK button below.", "Activate Account", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
                    {
                        EnterPassword enterPassword = new EnterPassword(attemptMember.getPassword);
                        if (enterPassword.ShowDialog() == true)
                        {
                            string newPassword = ((MainWindow)Application.Current.MainWindow)._temporaryPassthroughString;
                            ((MainWindow)Application.Current.MainWindow)._temporaryPassthroughString = null;

                            controller.ActivateMember(attemptMember._cardNumber, attemptMember.getPassword);
                            controller.UpdateMemberPassword(attemptMember._cardNumber, attemptMember.getPassword, newPassword);

                            MessageBox.Show("You have successfully activated your account! Please enter in your card number and your new password to get started!", "Activate Account");
                        }
                    }
                }
                else
                {
                    NavigationService.Navigate(new MemberMainpage(attemptMember));
                }
                txtCardNo.Text = null;
                animation = null;
                lblWrongDetails.BeginAnimation(Label.OpacityProperty, animation);
            }
            else
            {
                lblWrongDetails.BeginAnimation(Label.OpacityProperty, animation);
                //Label.OpacityProperty is the opacity of the label. animation goes from 1 to 0.
                //So what happens here is that the label goes from an opacity of 1.0 (fully opaque) to 0.0 (fully transparent) in 2 seconds
            }
        }

        private void pageLoaded(object sender, RoutedEventArgs e)
        {
            ((MainWindow)Application.Current.MainWindow)._temporaryPassthroughString = null;
            if (NavigationService.CanGoBack)
            {
                NavigationService.RemoveBackEntry();
                NavigationService.GoBack();
            }
            //This is done to remove the forward entry in the navigation service, which would give access to the account
            //The forward entry here is now simply another logout screen
        }
    }
}
