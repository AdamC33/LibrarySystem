﻿using System;
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
            XmlController controller = new XmlController();
            Member attemptMember = controller.GetMember(txtCardNo.Text, password.Password);
            if (attemptMember != null)
            {
                password.Password = null;
                if (txtCardNo.Text == "000000000")
                {
                    NavigationService.Navigate(new LibrarianMainpage());
                }
                else
                {
                    NavigationService.Navigate(new MemberMainpage(attemptMember));
                }
                txtCardNo.Text = null;
                lblWrongDetails.Content = null;
            }
            else
            {
                lblWrongDetails.Content = "Incorrect card number or password!";
            }
        }

        private void pageLoaded(object sender, RoutedEventArgs e)
        {
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
