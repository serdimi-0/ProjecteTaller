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
    /// Lógica de interacción para VehicleUC.xaml
    /// </summary>
    public partial class VehicleUC : UserControl
    {
        public VehicleUC()
        {
            InitializeComponent();
        }



        public Vehicle myVehicle
        {
            get { return (Vehicle)GetValue(myVehicleProperty); }
            set { SetValue(myVehicleProperty, value); }
        }

        // Using a DependencyProperty as the backing store for myVehicle.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty myVehicleProperty =
            DependencyProperty.Register("myVehicle", typeof(Vehicle), typeof(VehicleUC), new PropertyMetadata(null));


    }
}
