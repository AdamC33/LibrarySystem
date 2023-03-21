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
        public ManageBooks()
        {
            InitializeComponent();
            DataSet dataSet = new DataSet();
            dataSet.ReadXml(@libraryPath);
            dgLibrary.ItemsSource = dataSet.Tables[0].DefaultView;
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            AddBook addBook = new AddBook();
            addBook.Show();
        }

        private void btnMod_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnRem_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
