﻿using System;
using mshtml;
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

namespace DesktopPutintsev.Pages
{
    /// <summary>
    /// Логика взаимодействия для ReportPage.xaml
    /// </summary>
    public partial class ReportPage : Page
    {
        public ReportPage(string html)
        {
            InitializeComponent();
            webViem.NavigateToString(html);
        }

        private void BtnPrint_Click(object sender, RoutedEventArgs e)
        {
            var document = (IHTMLDocument2)webViem.Document;
            document.execCommand("print");
        }
    }
}
