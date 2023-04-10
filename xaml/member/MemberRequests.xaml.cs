using System;
using System.Collections.Generic;
using System.Diagnostics;
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
    /// Interaction logic for MemberRequests.xaml
    /// </summary>
    public partial class MemberRequests : Page
    {
        private Member _thisMember;
        private MemberMainpage _parentPage;

        public class requestDisplay
        {
            public string request { get; set; }
            public int index { get; set; }
        }

        public MemberRequests(Member thisMember, MemberMainpage parent)
        {
            //This page needs to interact with MemberMainpage in order for the notification bell exclamation mark to update in real time
            //The only way I have found of doing this is passing in the MemberMainpage page object when initialising the MemberRequests page
            //Using this.Parent gives a null result, not even giving the frame (frameMember) which this page is displayed in
            InitializeComponent();
            _thisMember = thisMember;
            _parentPage = parent;
            UpdateDisplay();
        }

        private void pageLoaded(object sender, RoutedEventArgs e)
        {
            NavigationService.RemoveBackEntry();
        }

        public void UpdateDisplay()
        {
            XmlController controller = new XmlController();
            _thisMember = controller.GetMember(_thisMember._cardNumber, _thisMember.getPassword);
            List<requestDisplay> requestsDisplay = new List<requestDisplay>();
            int i = -1;
            foreach (string r in _thisMember.getRequests)
            {
                i++;
                requestsDisplay.Add(new requestDisplay
                {
                    request = r,
                    index = i
                });
            }
            requestsDisplay.Reverse();
            listRequests.ItemsSource = requestsDisplay;
            _parentPage.UpdateDisplay(_thisMember);
        }

        private void btnRead_Click(object sender, RoutedEventArgs e)
        {
            XmlController controller = new XmlController();
            int index = Convert.ToInt32(((Button)sender).Uid);
            controller.ReadNotification(_thisMember._cardNumber, _thisMember.getPassword, index);
            UpdateDisplay();
        }

        private void btnAllRead_Click(object sender, RoutedEventArgs e)
        {
            XmlController controller = new XmlController();
            controller.ReadNotification(_thisMember._cardNumber, _thisMember.getPassword, -1);
            UpdateDisplay();
        }
    }
}
