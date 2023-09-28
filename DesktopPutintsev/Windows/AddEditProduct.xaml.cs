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
using System.Windows.Shapes;
using System.IO;
using Microsoft.Win32;

namespace DesktopPutintsev.Windows
{
    /// <summary>
    /// Логика взаимодействия для AddEditProduct.xaml
    /// </summary>
    public partial class AddEditProduct : Window
    {
        Model.Product _product;
        private byte[] _photo = null;
        bool _save = false;
        public AddEditProduct(Model.Product product)
        {
            InitializeComponent();
            var categoryList = App._Context.ProductCategory.ToList();
            var manufList = App._Context.Manufacturer.ToList();
            var providerList = App._Context.Provider.ToList();
            CmbProvider.ItemsSource = providerList;
            CmbCategory.ItemsSource = categoryList;
            CmbManuf.ItemsSource = manufList;
            if(product == null)
            {
                _save = true;
                _product = new Model.Product();
                Label.Content = "Добавление";
                Title = "Добавление";
            }
            else
            {
                _product = product;
                _photo = _product.ProductPhoto;
                Label.Content = "Редактирование";
                Title = "Редактирование";
            }
            DataContext = _product;
        }

        private void BtnGetImage_Click(object sender, RoutedEventArgs e)
        {
            var ofd = new OpenFileDialog
            {
                Filter = "Image files | *.png; *.jpg; *.jpeg;"
            };
            if (ofd.ShowDialog().Value)
            {
                _photo = File.ReadAllBytes(ofd.FileName);
                ImagePhoto.Source = new BitmapImage(new Uri(ofd.FileName));
                ImagePhoto.DataContext = _photo;
            }
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            decimal num;
            var prod = this.DataContext as Model.Product;
            string error = "";
            if (string.IsNullOrEmpty(prod.ProductArticleNumber))
                error += "Вы не ввели артикль\n";
            if (string.IsNullOrEmpty(prod.ProductName))
                error += "Вы не ввели название\n";
            if (string.IsNullOrEmpty(prod.ProductDescription))
                error += "Вы не ввели описание\n";
            if (prod.ProductCategory1 == null)
                error += "Вы не выбрали категории\n";
            if (prod.Manufacturer == null)
                error += "Вы не выбрали фирму\n";
            if (prod.Provider == null)
                error += "Вы не выбрали поставщика\n";
            //if (!decimal.TryParse(TbCost.Text, out num))
              //  error += "Неверный формат цены\n";
            if (prod.ProductCost <= 0 || string.IsNullOrWhiteSpace(prod.ProductCost.ToString()))
                error += "Неверная цена\n";
            if (prod.ProductDiscountAmount < 0 || prod.ProductDiscountAmount > prod.ProductMaxDiscount || string.IsNullOrWhiteSpace(prod.ProductDiscountAmount.ToString()))
                error += "Неверная скидка\n";
            if (!decimal.TryParse(TbDiscount.Text, out num))
                error += "Неверный формат скидки\n";
            if (prod.ProductDiscountAmount > prod.ProductMaxDiscount)
                error += "Слишком большая скидка";
            if (!decimal.TryParse(TbMaxDis.Text, out num))
                error += "Неверный формат макс.скидки\n";
            if (prod.ProductQuantityInStock < 0 || string.IsNullOrWhiteSpace(prod.ProductQuantityInStock.ToString()))
                error += "Неверное кол-во\n";
            if (!decimal.TryParse(TbQuant.Text, out num))
                error += "Неверный формат кол-во\n";
            if (prod.ProductMaxDiscount < 0 || prod.ProductMaxDiscount > 100 || string.IsNullOrWhiteSpace(prod.ProductMaxDiscount.ToString()))
                error += "Неверная максимальная скидка\n";
            if (!string.IsNullOrEmpty(error))
            {
                MessageBox.Show(error);
                error = "";
            }
            else
            {
                if (_save != true)
                {
                    _product.ProductPhoto = _photo;
                    App._Context.SaveChanges();
                    DialogResult = true;
                }
                else
                {
                    _product.ProductPhoto = _photo;
                    App._Context.Product.Add(_product);
                    App._Context.SaveChanges();
                    DialogResult = true;
                }
            }
        }
    }
}
