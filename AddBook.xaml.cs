using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
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
    /// Interaction logic for AddBook.xaml
    /// </summary>
    public partial class AddBook : Window
    {
        private bool _modifying = true; //Flag for if a book is being modified. If the flag is false, a book is being created.

        public AddBook(string isbn = null, string title = null, string author = null, string year = null, string publisher = null, string category = null)
        {
            InitializeComponent();
            if (isbn == null)
            {
                txtISBN.IsEnabled = true;
                _modifying = false;
            }
            else
            {
                txtISBN.Text = isbn;
                txtTitle.Text = title;
                txtAuthor.Text = author;
                txtYear.Text = year;
                txtPublisher.Text = publisher;
                txtCategory.Text = category;
            }
        }

        private void btnConfirm_Click(object sender, RoutedEventArgs e)
        {
            btnConfirm.IsEnabled = false;
            XmlController controller = new XmlController();
            List<Book> matchingISBN = controller.GetLibrary(txtISBN.Text, "isbn");
            if (matchingISBN.Count > 0 && !_modifying)
            {
                //Cannot create book - conflicting ISBN
            }
            else
            {
                Book newBook = new Book(
                txtISBN.Text,
                txtTitle.Text,
                txtAuthor.Text,
                txtYear.Text,
                txtPublisher.Text,
                txtCategory.Text,
                1 );

                if (_modifying) { controller.UpdateBook(txtISBN.Text, newBook); }
                else { controller.AddBook(newBook); }
            }
            btnConfirm.IsEnabled = true;
            this.Close();
        }

        private void checkTextBoxes(object sender = null, TextChangedEventArgs e = null)
        {
            //Enables confirm button to be clicked if there is some text in all boxes
            if (txtISBN.Text.Length == 13
                && txtTitle.Text.Length > 0
                && txtAuthor.Text.Length > 0
                && txtYear.Text.Length > 0
                && txtPublisher.Text.Length > 0
                && txtCategory.Text.Length > 0)
            {
                btnConfirm.IsEnabled = true;
            }
            else
            {
                btnConfirm.IsEnabled = false;
            }
        }

        private void txtISBN_TextChanged(object sender, TextChangedEventArgs e)
        {
            //This code only allows numbers to be entered into the ISBN textbox
            int initialSelectionStart = txtISBN.SelectionStart;
            string txtISBNString = txtISBN.Text;
            foreach (char c in txtISBN.Text)
            {
                if (!Char.IsDigit(c))
                {
                    //If the character isn't a digit, it gets removed from the string
                    txtISBNString = txtISBNString.Remove(txtISBNString.IndexOf(c), 1);
                    initialSelectionStart--;
                }
            }
            txtISBN.Text = txtISBNString;
            txtISBN.SelectionStart = initialSelectionStart;

            checkTextBoxes();
        }

        private void txtYear_TextChanged(object sender, TextChangedEventArgs e)
        {
            //This code only allows numbers to be entered into the year textbox
            int initialSelectionStart = txtYear.SelectionStart;
            string txtYearString = txtYear.Text;
            foreach (char c in txtYear.Text)
            {
                if (!Char.IsDigit(c))
                {
                    //If the character isn't a digit, it gets removed from the string
                    txtYearString = txtYearString.Remove(txtYearString.IndexOf(c), 1);
                    initialSelectionStart--;
                }
            }
            txtYear.Text = txtYearString;
            txtYear.SelectionStart = initialSelectionStart;

            checkTextBoxes();
        }
    }
}
