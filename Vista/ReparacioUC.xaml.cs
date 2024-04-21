using Model;
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

namespace Vista
{
    /// <summary>
    /// Lógica de interacción para ReparacioUC.xaml
    /// </summary>
    public partial class ReparacioUC : UserControl
    {
        public ReparacioUC()
        {
            InitializeComponent();
        }



        public Reparacio myReparacio
        {
            get { return (Reparacio)GetValue(myReparacioProperty); }
            set { SetValue(myReparacioProperty, value); }
        }

        // Using a DependencyProperty as the backing store for myReparacio.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty myReparacioProperty =
            DependencyProperty.Register("myReparacio", typeof(Reparacio), typeof(ReparacioUC), new PropertyMetadata(null));


    }
}
