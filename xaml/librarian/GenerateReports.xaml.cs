using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.IO.Pipes;
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

namespace LibrarySystem.xaml.librarian
{
    /// <summary>
    /// Interaction logic for GenerateReports.xaml
    /// </summary>
    public partial class GenerateReports : Page
    {
        private class PopularBookDisplay
        {
            public string bookISBN { get; set; }
            public string bookTitle { get; set; }
            public int numberOfCheckoutsAndQueue { get; set; }
        }

        private class OverdueBookDisplay
        {
            public string memberCardNumber { get; set; }
            public string memberName { get; set; }
            public string bookISBN { get; set; }
            public string bookTitle { get; set; }
            private string dueDateString { get; set; }

            public DateTimeOffset dueDate
            {
                set { dueDateString = value.DateTime.ToString(CultureInfo.CreateSpecificCulture("en-GB")); }
            }

            public string getDueDate
            {
                get { return dueDateString; }
            }
        }

        public GenerateReports()
        {
            InitializeComponent();
            DateTime currentTime = DateTime.Now;
            int numberOfBooksCheckedOut = 0;
            int lengthOfQueuesForAllBooks = 0;
            UInt32 totalStockOfAllBooks = 0;
            List<OverdueBookDisplay> overdueBooks = new List<OverdueBookDisplay>();
            XmlController controller = new XmlController();
            List<Book> library = controller.GetLibrary();
            foreach (Book b in library)
            {
                numberOfBooksCheckedOut += b.checkoutListMinusQueueLength;
                lengthOfQueuesForAllBooks += b.queueForBook;
                totalStockOfAllBooks += b._totalStock;
                foreach (string cardNumber in b.membersWithOverdueBook)
                {
                    overdueBooks.Add(new OverdueBookDisplay
                    {
                        memberCardNumber = cardNumber,
                        memberName = controller.GetMemberName(cardNumber),
                        bookISBN = b._ISBN,
                        bookTitle = b._title,
                        dueDate = b.getDueDate(cardNumber)
                    });
                }
            }

            string popularBooksString = "";
            IEnumerable<Book> booksSortedByPopularityIEnumerable = library.OrderByDescending(o => o.checkoutListLength); 
            //Using an IEnumerable is the quickest way I found of ordering a list of objects using one of their properties (in this case the number of checkouts) as the way of ordering it
            List<Book> booksSortedByPopularity = booksSortedByPopularityIEnumerable.ToList();
            for (int i = 0; i < 10; i++)
            {
                popularBooksString += String.Format("{0} ({1}) - Number of members with the book checked out (or in queue for): {2}\n", booksSortedByPopularity[i]._title, booksSortedByPopularity[i]._ISBN, booksSortedByPopularity[i].checkoutListLength);
            }
            if (popularBooksString.Length == 0) { popularBooksString = "No books in library!"; }

            string overdueBooksString = "";
            foreach (OverdueBookDisplay b in overdueBooks)
            {
                overdueBooksString += String.Format("{0} ({1}) - Due Date {2} - {3} ({4})\n", b.bookTitle, b.bookISBN, b.getDueDate, b.memberName, b.memberCardNumber);
            }
            if (overdueBooksString.Length == 0) { overdueBooksString = "No overdue books! "; }

            txtReport.Text =
            (
            "Library Bookings Co. report as of " + currentTime + "\n\n" +
            "Number of books checked out: " + numberOfBooksCheckedOut + "\n" +
            "Length of queues for all books: " + lengthOfQueuesForAllBooks + "\n" +
            "Total stock of all books: " + totalStockOfAllBooks + "\n\n" +
            "Ten most popular books: \n" +
            popularBooksString + "\n" +
            "Overdue books: \n" +
            overdueBooksString
            );
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.SaveFileDialog saveFileDlg = new Microsoft.Win32.SaveFileDialog();
            saveFileDlg.FileName = "Report";
            saveFileDlg.DefaultExt = ".txt";
            saveFileDlg.Filter = "Text File|*.txt";
            if (saveFileDlg.ShowDialog() == true)
            {
                StreamWriter reportFile = new StreamWriter(saveFileDlg.FileName);
                reportFile.Write(txtReport.Text);
                reportFile.Close();
            }
        }
    }
}
