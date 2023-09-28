using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace DesktopPutintsev
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static Model.PutintsevUPEntities _Context { get; } = new Model.PutintsevUPEntities();
        public static Model.User CurrentUser { get; set; } = null;
    }
}
