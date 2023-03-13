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
            //The "newest books" in this case are the ones that are furthest down the XML file
            frameBook1.Content = new BookFrameHomepage();
            frameBook2.Content = new BookFrameHomepage();
            frameBook3.Content = new BookFrameHomepage();
            frameBook4.Content = new BookFrameHomepage();
            try
            {
                frameBook1.Content = new BookFrameHomepage(
                    library[library.Count() - 1]._title,
                    library[library.Count() - 1]._author,
                    library[library.Count() - 1]._year,
                    library[library.Count() - 1]._publisher,
                    library[library.Count() - 1]._category );

                frameBook2.Content = new BookFrameHomepage(
                    library[library.Count() - 2]._title,
                    library[library.Count() - 2]._author,
                    library[library.Count() - 2]._year,
                    library[library.Count() - 2]._publisher,
                    library[library.Count() - 2]._category );

                frameBook3.Content = new BookFrameHomepage(
                    library[library.Count() - 3]._title,
                    library[library.Count() - 3]._author,
                    library[library.Count() - 3]._year,
                    library[library.Count() - 3]._publisher,
                    library[library.Count() - 3]._category );

                frameBook4.Content = new BookFrameHomepage(
                    library[library.Count() - 4]._title,
                    library[library.Count() - 4]._author,
                    library[library.Count() - 4]._year,
                    library[library.Count() - 4]._publisher,
                    library[library.Count() - 4]._category );
            }
            catch (ArgumentOutOfRangeException)
            { 
                //If there are less than 4 books in the XML file, this acts as a failsafe so that it displays "TBA" in all of the fields instead of the program crashing
            }
        }
    }
}
