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
    /// Логика взаимодействия для WatchOrderPage.xaml
    /// </summary>
    public partial class WatchOrderPage : Page
    {
        Model.Order _order;
        public WatchOrderPage(Model.Order order)
        {
            InitializeComponent();
            _order = order;
            DataContext = _order;
            if (order.OrderStatus1.StatusName == "Завершен")
                BtnSetStatus.IsEnabled = false;
            List<Model.OrderProduct> orderProducts = new List<Model.OrderProduct>();
            foreach(var item in App._Context.OrderProduct.ToList())
            {
                if(order.OrderID == item.OrderID)
                    orderProducts.Add(item);
            }
            ProdList.ItemsSource = orderProducts;
        }

        private void BtnSetStatus_Click(object sender, RoutedEventArgs e)
        {
            var orders = this.DataContext as Model.Order;
            orders.OrderStatus = 1;
            App._Context.SaveChanges();
            Update(orders);
            DataContext = null;
            DataContext = orders;
        }

        private void Update(Model.Order order)
        {
            if (order.OrderStatus1.StatusName == "Завершен")
                BtnSetStatus.IsEnabled = false;
            List<Model.OrderProduct> orderProduct = new List<Model.OrderProduct>();
            foreach (var item in App._Context.OrderProduct.ToList())
            {
                if (order.OrderID == item.OrderID)
                    orderProduct.Add(item);
            }
            ProdList.ItemsSource = orderProduct;
        }

        private void BtnReport_Click(object sender, RoutedEventArgs e)
        {
            decimal fullcost = 0;
            var products = ProdList.ItemsSource as List<Model.OrderProduct>;
            var orders = this.DataContext as Model.Order;
            StringBuilder builder = new StringBuilder();
            builder.Append("<!DOCTYPE html>\n");
            builder.Append("<html>\n");
            builder.Append("<head>\n" +
                "<meta charset=\"utf-8\">\n" +
                "<style type=\"text/css\">\n" +
                "#invoice-POS{\n" +
  "box - shadow: 0 0 1in -0.25in rgba(0, 0, 0, 0.5);\n" +
        "padding: 2mm;\n" +
        "margin: 0 auto;\n" +
        "width: 44mm;\n" +
        "background: #FFF;\n" +
"::selection {background: #f31544; color: #FFF;}\n" +
"::moz-selection {background: #f31544; color: #FFF;}\n" +
"h1{\n" +
                    "font - size: 1.5em;\n" +
                    "color: #222;\n" +
"}\n" +
                    "h2{ font - size: .9em; }\n" +
                    "h3{\n" +
                        "font - size: 1.2em;\n" +
                        "font - weight: 300;\n" +
                        "line - height: 2em;\n" +
                    "}\n" +
                    "p{\n" +
                        "font - size: .7em;\n" +
                        "color: #666;\n" +
                        "line - height: 1.2em;\n" +
                    "}\n" +

"#top, #mid,#bot{\n" +
                    "border-bottom: 1px solid #EEE;\n" +
"}\n" +

"#mid{min-height: 80px;}\n" +
"#bot{ min-height: 50px;}\n" +

".info{\n" +
                "display: block;\n" +
                "margin - left: 0;\n" +
                "}\n" +
".title{\n" +
                    "float: right;\n" +
                "}\n" +
".title p{ text - align: right;}\n" +
                "table{\n" +
                "width: 100 %;\n" +
                    "border - collapse: collapse;\n" +
                "}\n" +
                "td{\n" +
                "}\n" +
".tabletitle{\n" +
                    "font - size: .5em;\n" +
                "background: #EEE;\n" +
"}\n" +
".service{border-bottom: 1px solid #EEE;}\n" +
                    ".item{ width: 24mm; }\n" +
                    ".itemtext{ font-size: .5em; }\n" +

"# legalcopy{\n" +
                    "margin-top: 5mm;\n" +
                "}\n" +
            "}\n" +
            "</style>\n" +
                 "</head>\n");
            builder.Append("<body>\n");
            builder.Append("<div id=\"invoice-POS\">");
            builder.Append("<center id=\"top\">");
            builder.Append("<div id=\"info\">");
            builder.Append("<h2>столовыйSTORE</h2>");
            builder.Append("</div>");
            builder.Append("</center>");
            builder.Append("<div id=\"mid\">");
            builder.Append("<div class=\"info\">");
            builder.Append("<h2>Контактная информация</h2>");
            builder.Append("<p>\nАдрес: я не придумал</br>\nEmail: Stoloviy@gmail.com</br>\nТелефон: +7-999-555-44-33</br>\n</p>");
            builder.Append("</div>");
            builder.Append("</div>");
            builder.Append("<div id=\"bot\">");
            builder.Append("<div id=\"table\">");
            builder.Append("<table>");
            builder.Append("<tr class=\"tabletitle\">");
            builder.Append("<td class=\"item\"><h3>Товар</h3></td>");
            builder.Append("<td class=\"hours\"><h3>Кол-во</h3></td>");
            builder.Append("<td class=\"price\"><h3>Цена</h3></td>");
            builder.Append("</tr>");
            builder.Append("<tr class=\"service\">");
            foreach (var prod in products)
            {
                builder.Append($"<td class=\"tableitem\"><p class=\"itemtext\"> {prod.Product.ProductName}</p></td>");
                builder.Append($"<td class=\"tableitem\"><p class=\"itemtext\"><strong> {prod.ProductCount}</strong></p></td>");
                builder.Append($"<td class=\"tableitem\"><p class=\"itemtext\"> {prod.Product.ProductCost}</p></td>\n</tr>");
            }
            builder.Append("</tr>");
            builder.Append("</tr>");
            builder.Append("<tr class=\"service\">");
            builder.Append("</tr>");
            builder.Append("<tr class=\"tabletitle\">");
            builder.Append("</tr>");
            builder.Append("<tr class=\"tabletitle\">");
            builder.Append("<td></td>");
            builder.Append("<td class=\"rate\"<h2>Сумма:</h2></td>");
            foreach (var prod in products)
            {
                fullcost += prod.ProductCount * prod.Product.ProductCost;
            }
            builder.Append($"<td class=\"payment\"<h2>{fullcost} руб.</h2></td>");
            builder.Append("</td>");
            builder.Append("</tr>");
            builder.Append("</table>");
            builder.Append("</div>");
            builder.Append("<div id=\"legalcopy\">");
            builder.Append($"<p class=\"legal\"<strong>Спасибо за покупку!</strong> Гарантия на товар 12 месяцев.\n<strong>Дата продажи: {_order.OrderCreateDate.ToShortDateString()}</strong></p>");
            builder.Append("</div>");
            builder.Append("</div>");
            builder.Append("</div>");
            builder.Append("</body>\n");
            builder.Append("</html>\n");
            NavigationService.Navigate(new ReportPage(builder.ToString()));
        }
    }
}
