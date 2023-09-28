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

namespace DesktopPutintsev
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            MainFrame.Navigate(new Pages.AuthorizationPage());
        }

        private void MainFrame_ContentRendered(object sender, EventArgs e)
        {
            Page page = MainFrame.Content as Page;

            if (page.Title == "Авторизация")
            {
                BTNBack.Visibility = Visibility.Collapsed;
                LbUser.Visibility = Visibility.Collapsed;
                App.CurrentUser = null;
            }
            else
            {
                LbUser.Visibility = Visibility.Visible;
                if(App.CurrentUser != null)
                    LbUser.Content = App.CurrentUser.FullName;
                BTNBack.Visibility = Visibility.Visible;
            }
        }

        private void BTNBack_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.GoBack();
        }
    }
}
