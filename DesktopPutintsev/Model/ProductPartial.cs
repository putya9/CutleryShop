using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace DesktopPutintsev.Model
{
    public partial class Product
    {
        public Brush BackColor 
        {
            get
            {
                if (ProductQuantityInStock > 0)
                    return Brushes.Green;
                else
                    return Brushes.Gray;
            }
        }
    }
}
