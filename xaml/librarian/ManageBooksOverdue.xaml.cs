using LibrarySystem.classes;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace LibrarySystem
{
    /// <summary>
    /// Interaction logic for ManageBooksOverdue.xaml
    /// </summary>
    public partial class ManageBooksOverdue : Page
    {
        public ManageBooksOverdue()
        {
            InitializeComponent();
            UpdateDisplay();
        }

        private void UpdateDisplay()
        {
            XmlController controller = new XmlController();
            List<Book> library = controller.GetLibrary();
            List<OverdueBooks> overdueBooks = new List<OverdueBooks>();
            //DateTimeOffset currentTime = DateTimeUK.DateTimeOffsetNow;
            foreach (Book b in library)
            {
                foreach (string cardNumber in b.membersWithOverdueBook)
                {
                    overdueBooks.Add(new OverdueBooks
                    {
                        memberCardNumber = cardNumber,
                        memberName = controller.GetMemberName(cardNumber),
                        bookISBN = b._ISBN,
                        bookTitle = b._title,
                        dueDate = b.getDueDate(cardNumber)
                    });
                }
            }
            listOverdueBooks.ItemsSource = overdueBooks;
        }

        private void btnNotify_Click(object sender, RoutedEventArgs e)
        {
            OverdueBooks thisBook = (OverdueBooks)((Button)sender).DataContext;
            OverdueBooks.notifyMember(thisBook.memberCardNumber, thisBook.bookTitle, thisBook.bookISBN);
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}
