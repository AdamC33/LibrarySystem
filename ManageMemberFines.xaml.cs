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
    /// Interaction logic for ManageMemberFines.xaml
    /// </summary>
    public partial class ManageMemberFines : Page
    {
        private Member _thisMember;

        public ManageMemberFines(Member thisMember)
        {
            InitializeComponent();
            _thisMember = thisMember;
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            FineAdd fineAdd = new FineAdd();
            fineAdd.ShowDialog(); //ShowDialog is used instead of Show as it pauses the main window.
            //Any code after this is will be run after the fineAdd window has closed (either because the user has clicked "Confirm" or the close button in the top right)
        }

        private void btnMod_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnRem_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
