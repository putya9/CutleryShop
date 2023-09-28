using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
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
using System.Windows.Threading;

namespace DesktopPutintsev.Pages
{
    /// <summary>
    /// Логика взаимодействия для AuthorizationPage.xaml
    /// </summary>
    public partial class AuthorizationPage : Page
    {
        private int _count = 0;
        private Random _rand = new Random();
        private string _text = "";
        private DispatcherTimer _timer = new DispatcherTimer()
        {
            Interval = TimeSpan.FromSeconds(10)
        };
        public AuthorizationPage()
        {
            InitializeComponent();
            _timer.Tick += _timer_Tick;
        }
        private void _timer_Tick(object sender, EventArgs e)
        {
            this.IsEnabled = true;
            _timer.Stop();
        }
        private void BTNLogin_Click_1(object sender, RoutedEventArgs e)
        {
            if (TBoxLogin.Text != "" && TBoxPassword.Password != "")
            {
                if (App._Context.User.FirstOrDefault(p => p.UserLogin == TBoxLogin.Text && p.UserPassword == TBoxPassword.Password) is Model.User user && TBxCapcha.Text == _text)
                {
                    App.CurrentUser = user;
                    if (user.UserRole != 3)
                        NavigationService.Navigate(new ContentPage());
                    else
                        NavigationService.Navigate(new ProductPage());
                    return;
                }
                else
                {
                    MessageBox.Show("Неверные данные!", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                    _count++;
                }
            }

            if(_count > 0)
            {
                ImageCapcha.Visibility = Visibility.Visible;
                TBxCapcha.Visibility = Visibility.Visible;
                _text = GenerateTextCapcha();
                ImageCapcha.Source = BitmapToImageSource(GenerateCapcha(150, 100, _text));
            }
            if(_count > 1)
            {
                this.IsEnabled = false;
                _timer.Start();
            }
        }

        private void ImageCapcha_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            _text = GenerateTextCapcha();
            ImageCapcha.Source = BitmapToImageSource(GenerateCapcha(150, 100, _text));
        }
        private string GenerateTextCapcha()
        {
            string str = "";
            for (int i = 0; i < 4; i++)
            {
                switch(_rand.Next(2))
                {
                    case 0:
                        str += RandomDigit();
                        break;
                    case 1:
                        str += RandomLetter();
                        break;
                    default:
                        break;
                }
            }
            return str;
        }
        private char RandomDigit()
        {
            return (char)_rand.Next(48, 58);
        }
        private char RandomLetter()
        {
            return (char)_rand.Next(97, 123);
        }
        private Bitmap GenerateCapcha(int width, int height, string text)
        {
            Bitmap bmp = new Bitmap(width, height);

            using(Graphics graphics = Graphics.FromImage(bmp))
            {
                graphics.FillRectangle(System.Drawing.Brushes.Black, 0, 0, bmp.Width, bmp.Height);
                for (int i = 0; i < text.Length; i++)
                {
                    Font font = new Font("Comic Sans MS", _rand.Next(48, 62));
                    graphics.DrawString(text[i].ToString(), font, System.Drawing.Brushes.Green, _rand.Next(25, 35) * i, _rand.Next(height / 4));
                    font.Dispose();
                }
                graphics.Flush();
                graphics.Dispose();
            }
            for (int x = 0; x < bmp.Width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    int q = _rand.Next(101);
                    if (q < 35)
                        bmp.SetPixel(x, y, System.Drawing.Color.Gray);
                }
            }
            return bmp;
        }
        private BitmapImage BitmapToImageSource(Bitmap bitmap)
        {
            using (MemoryStream memory = new MemoryStream())
            {
                bitmap.Save(memory, System.Drawing.Imaging.ImageFormat.Bmp);
                memory.Position = 0;
                BitmapImage bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = memory;
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.EndInit();
                return bitmapImage;
            }
        }

        private void BTNLoginGues_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new ProductPage());
            return;
        }
    }
}
