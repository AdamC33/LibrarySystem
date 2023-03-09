﻿using System;
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
    /// Interaction logic for MemberMainpage.xaml
    /// </summary>
    public partial class MemberMainpage : Page
    {
        public MemberMainpage(Member currentUser)
        {
            InitializeComponent();
            frameMember.Content = new MemberHomepage();
        }
    }
}
