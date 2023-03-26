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
    /// Interaction logic for AddMember.xaml
    /// </summary>
    public partial class AddMember : Window
    {
        private bool _modifying = true;
        private string _oldCardNumber;

        public AddMember(string cardNumber = null, string name = null, string phoneNumber = null, string email = null)
        {
            InitializeComponent();
            if (cardNumber == null)
            {
                _modifying = false;
            }
            else
            {
                _oldCardNumber = cardNumber;
                txtCardNumber.Text = cardNumber;
                txtName.Text = name;
                txtPhoneNo.Text = phoneNumber;
                txtEmail.Text = email;
            }
        }

        private void btnConfirm_Click(object sender, RoutedEventArgs e)
        {
            btnConfirm.IsEnabled = false;
            XmlController controller = new XmlController();
            if (controller.GetMemberName(txtCardNumber.Text) != null && txtCardNumber.Text != _oldCardNumber)
            {
                MessageBox.Show("Cannot save member - conflicting card number!", "Save Member");
            }
            else
            {
                Member newMember = new Member(
                txtISBN.Text,
                txtTitle.Text,
                txtAuthor.Text,
                txtYear.Text,
                txtPublisher.Text,
                txtCategory.Text,
                1);

                if (_modifying) { controller.UpdateBook(_oldISBN, newBook); }
                else { controller.AddBook(newBook); }
                this.Close();
            }
            btnConfirm.IsEnabled = true;
        }
    }
}
