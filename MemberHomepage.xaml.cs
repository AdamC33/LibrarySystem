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
    public partial class MemberHomepage : Page
    {
        public MemberHomepage()
        {
            InitializeComponent();
            XmlController controller = new XmlController();
            List<Book> library = controller.GetLibrary();

            //Sets up the new books on the homepage
            lblBook1Title.Content = library[library.Count() - 1]._title;
            lblBook1Author.Content = library[library.Count() - 1]._author;
            lblBook2Title.Content = library[library.Count() - 2]._title;
        }
    }
}
