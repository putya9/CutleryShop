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

namespace DesktopPutintsev.Pages
{
    /// <summary>
    /// Логика взаимодействия для SalesPage.xaml
    /// </summary>
    public partial class SalesPage : Page
    {
        private readonly List<Model.Order> orders = App._Context.Order.ToList();
        public SalesPage()
        {
            InitializeComponent();
            SalesList.ItemsSource = orders;
            var list = App._Context.OrderStatus.ToList();
            list.Insert(0, new Model.OrderStatus
            {
                StatusName = "Фильтрация по статусу"
            });
            CmbFiltr.ItemsSource = list;
            CmbFiltr.SelectedIndex = 0;
            DTDate.SelectedDate = DateTime.Now;
        }
        private void UpdateData()
        {
            var orders = App._Context.Order.ToList();
            if(CmbFiltr.SelectedIndex != 0)
            {
                orders = orders.Where(p => p.OrderStatus1 == CmbFiltr.SelectedItem as Model.OrderStatus).ToList();
                SalesList.ItemsSource = orders;
            }
            else
                SalesList.ItemsSource = orders;
            orders = orders.Where(p => p.OrderID.ToString().Contains(TBSearch.Text.ToLower())
            || p.OrderPickPoint.PostIndex.ToLower().Contains(TBSearch.Text.ToLower())
            || p.OrderStatus1.StatusName.ToLower().Contains(TBSearch.Text.ToLower())
            || p.CodeForDelivery.ToLower().Contains(TBSearch.Text.ToLower())
            || (p.User != null && p.User.UserSurname.ToLower().Contains(TBSearch.Text.ToLower()))
            || (p.User != null && p.User.UserName.ToLower().Contains(TBSearch.Text.ToLower()))
            || (p.User != null && p.User.UserPatronymic.ToLower().Contains(TBSearch.Text.ToLower()))).ToList();
            orders = orders.Where(p => p.OrderCreateDate <= DTDate.SelectedDate?.Date).ToList();
            SalesList.ItemsSource = orders;
        }
        private void TBSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateData();
        }

        private void CmbFiltr_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateData();
        }

        private void BtnWatch_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new WatchOrderPage((sender as Button).DataContext as Model.Order));
        }

        private void DTDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateData();
        }
    }
}
