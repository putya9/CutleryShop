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
    /// Логика взаимодействия для BasketPage.xaml
    /// </summary>
    public partial class BasketPage : Page
    {
        List<Model.OrderProduct> _product;
        private Random _rand = new Random();
        public BasketPage(List<Model.OrderProduct> products)
        {
            InitializeComponent();
            var sortList = new List<string>()
            {
                "Без сортировки",
                "Цена (убыв)",
                "Цена (возр)"
            };
            var pickPoint = App._Context.OrderPickPoint.ToList();
            CmbPickPoint.ItemsSource = pickPoint;
            CmbSort.ItemsSource = sortList;
            _product = products;
            BasketList.ItemsSource = products;
        }
        private void UpdateData()
        {
            var product = _product;
            switch (CmbSort.SelectedIndex)
            {
                case 1:
                    product = product.OrderByDescending(p => (p.Product.ProductCost)).ToList();
                    break;
                case 2:
                    product = product.OrderBy(p => (p.Product.ProductCost)).ToList();
                    break;
                default:
                    product = product.OrderBy(p => p.Product.ProductName).ToList();
                    break;
            }
            BasketList.ItemsSource = product;
        }
        private void BtnDel_Click(object sender, RoutedEventArgs e)
        {
            _product.Remove((sender as Button).DataContext as Model.OrderProduct);
            UpdateData();
        }

        private void CmbSort_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateData();
        }

        private void BtnBuy_Click(object sender, RoutedEventArgs e)
        {
            Model.Order order = new Model.Order();
            order.OrderStatus = 2;
            order.OrderPickupPoint = CmbPickPoint.SelectedIndex + 1;
            order.OrderCreateDate = DateTime.Now;
            order.OrderDeliveryDate = DateTime.Now.AddDays(7);
            order.User = App.CurrentUser;
            Random _rand = new Random();
            int code = _rand.Next(100, 1000);
            order.CodeForDelivery = code.ToString();
            order.OrderProduct = _product;
            App._Context.Order.Add(order);
            App._Context.SaveChanges();
            MessageBox.Show($"Заказ успешно сформирован");
            _product = null;
            BasketList.ItemsSource = null;
            BasketList.ItemsSource = _product;
            order = null;
        }
    }
}
