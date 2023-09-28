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
    /// Логика взаимодействия для ProductPage.xaml
    /// </summary>
    public partial class ProductPage : Page
    {
        private readonly List<Model.Product> products = App._Context.Product.ToList();
        List<Model.OrderProduct> productsList = new List<Model.OrderProduct>();
        public ProductPage()
        {
            InitializeComponent();
            if(App.CurrentUser != null)
            {
                switch (App.CurrentUser.Role.RoleName.ToLower())
                {
                    case "администратор":
                        BTNAdd.Visibility = Visibility.Visible;
                        break;
                    default:
                        BTNAdd.Visibility = Visibility.Collapsed;
                        break;

                }
            }
            else
                BTNAdd.Visibility = Visibility.Collapsed;
            ProductList.ItemsSource = products;
            var sortList = new List<string>()
            {
                "Без сортировки",
                "Цена (убыв)",
                "Цена (возр)"
            };
            var list = App._Context.Manufacturer.ToList();
            list.Insert(0, new Model.Manufacturer
            {
                ManufacturerName = "Все прозводители"
            });
            CmbMan.ItemsSource = list;
            CmbSort.ItemsSource = sortList;
            CmbMan.SelectedIndex = 0;
            CmbSort.SelectedIndex = 0;
            TBnow.Text = $"{App._Context.Product.Count()}";
        }
        private void UpdateData()
        {
            var product = App._Context.Product.ToList();
            switch (CmbSort.SelectedIndex)
            {
                case 1:
                    product = product.OrderByDescending(p => (p.ProductCost)).ToList();
                    break;
                case 2:
                    product = product.OrderBy(p => (p.ProductCost)).ToList();
                    break;
                default:
                    product = product.OrderBy(p => p.ProductName).ToList();
                    break;
            }
            if(CmbMan.SelectedIndex != 0)
            {
                product = product.Where(p => p.Manufacturer == CmbMan.SelectedItem as Model.Manufacturer).ToList();
                ProductList.ItemsSource = product;
            }
            else
            {
                ProductList.ItemsSource = product;
            }
            product = product.Where(p => p.Manufacturer.ManufacturerName.ToLower().Contains(TbSearch.Text.ToLower())
            || p.ProductName.ToLower().Contains(TbSearch.Text.ToLower())
            || p.ProductDescription.ToLower().Contains(TbSearch.Text.ToLower())
            || p.ProductCost.ToString().Contains(TbSearch.Text.ToLower())).ToList();
            TBCount.Text = $"{product.Count}";
            ProductList.ItemsSource = product;
        }
        private void CmbSort_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateData();
        }

        private void TbSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateData();
        }

        private void CmbMan_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateData();
        }

        private void BtnDel_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Действительно удалить?", "Удалить", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                App._Context.Product.Remove((sender as Button).DataContext as Model.Product);
                App._Context.SaveChanges();
                UpdateData();
            }
        }

        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            Window window = new Windows.AddEditProduct((sender as Button).DataContext as Model.Product);
            if (window.ShowDialog() == true)
                UpdateData();
        }

        private void BTNAdd_Click(object sender, RoutedEventArgs e)
        {
            Window window = new Windows.AddEditProduct(null);
            if(window.ShowDialog() == true)
                UpdateData();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            UpdateData();
        }

        private void BtnBuy_Click(object sender, RoutedEventArgs e)
        {
            productsList.Add((sender as Button).DataContext as Model.OrderProduct);
        }

        private void BtnBasket_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new BasketPage(productsList));
        }
    }
}
