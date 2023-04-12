using System;
using System.Collections.Generic;
using System.Data;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace LibrarySystem
{
    /// <summary>
    /// Interaction logic for ManageBooks.xaml
    /// </summary>
    public partial class ManageBooks : Page
    {
        private string libraryPath = "Library.xml";
        private DataSet dataSet = new DataSet();

        public ManageBooks()
        {
            InitializeComponent();
            dataSet.ReadXml(@libraryPath);
            dgLibrary.ItemsSource = dataSet.Tables[0].DefaultView;
        }

        private void dgLibrary_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgLibrary.SelectedItem != null)
            {
                btnMod.IsEnabled = true;
                btnRem.IsEnabled = true;
                btnStock.IsEnabled = true;
            }
            else
            {
                btnMod.IsEnabled = false;
                btnRem.IsEnabled = false;
                btnStock.IsEnabled = false;
            }
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private void btnStock_Click(object sender, RoutedEventArgs e)
        {
            DataRowView row = dgLibrary.SelectedItem as DataRowView;
            XmlController controller = new XmlController();
            Book thisBook = controller.GetLibrary(row.Row.ItemArray[4].ToString(), "isbn")[0];
            NavigationService.Navigate(new ManageBookStock(thisBook));

            dataSet.Reset();
            dataSet.ReadXml(@libraryPath);
            dgLibrary.ItemsSource = dataSet.Tables[0].DefaultView;
        }

        private void btnOverdueBooks_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new ManageBooksOverdue());
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            AddBook addBook = new AddBook();
            addBook.ShowDialog(); //ShowDialog is used instead of Show as it pauses the main window.
            //Any code after this is will be run after the addBook window has closed (either because the user has clicked "Confirm" or the close button in the top right)
            dataSet.Reset();
            dataSet.ReadXml(@libraryPath);
            dgLibrary.ItemsSource = dataSet.Tables[0].DefaultView;
        }

        private void btnMod_Click(object sender, RoutedEventArgs e)
        {
            DataRowView row = dgLibrary.SelectedItem as DataRowView;
            AddBook modBook = new AddBook(
            row.Row.ItemArray[4].ToString(),
            row.Row.ItemArray[0].ToString(),
            row.Row.ItemArray[1].ToString(),
            row.Row.ItemArray[2].ToString(),
            row.Row.ItemArray[3].ToString(),
            row.Row.ItemArray[5].ToString() );
            modBook.ShowDialog();

            dataSet.Reset();
            dataSet.ReadXml(@libraryPath);
            dgLibrary.ItemsSource = dataSet.Tables[0].DefaultView;
        }

        private void btnRem_Click(object sender, RoutedEventArgs e)
        {
            XmlController controller = new XmlController();
            DataRowView row = dgLibrary.SelectedItem as DataRowView;
            Book selectedBook = controller.GetLibrary(row.Row.ItemArray[4].ToString(), "isbn")[0];
            if (selectedBook.checkoutListLength > 0)
            {
                MessageBox.Show("Cannot delete book, people have it checked out!", "Delete Book");
            }
            else
            {
                MessageBoxResult yesOrNo = MessageBox.Show("Are you sure you want to delete this book?", "Delete Book", MessageBoxButton.YesNo);
                if (yesOrNo == MessageBoxResult.Yes)
                {
                    controller.DeleteBook(row.Row.ItemArray[4].ToString());
                }

                dataSet.Reset();
                dataSet.ReadXml(@libraryPath);
                dgLibrary.ItemsSource = dataSet.Tables[0].DefaultView;
            }
        }
    }
}
