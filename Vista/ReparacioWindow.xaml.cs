using InterficiePersistencia;
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
using System.Windows.Shapes;

namespace Vista
{
    /// <summary>
    /// Lógica de interacción para ReparacioWindow.xaml
    /// </summary>
    public partial class ReparacioWindow : Window
    {
        Reparacio reparacio;
        GestorBDTaller cp;

        public ReparacioWindow(Reparacio reparacio, GestorBDTaller cp)
        {
            this.reparacio = reparacio;
            this.cp = cp;
            cp.obtenirLinies(reparacio);
            WindowStartupLocation = WindowStartupLocation.CenterOwner;
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            txbEstat.Text = reparacio.EstatString;
            txbData.Text = reparacio.DataString;
            txbMatricula.Text = reparacio.VehicleId;
            txbModel.Text = reparacio.Model;
            lsvLinies.ItemsSource = reparacio.Linies;
        }
    }
}
