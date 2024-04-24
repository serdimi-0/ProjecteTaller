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
    /// Lógica de interacción para SeleccioVehicleWindow.xaml
    /// </summary>
    public partial class SeleccioVehicleWindow : Window
    {
        Client client;
        Usuari usuari;
        GestorBDTaller cp;

        public SeleccioVehicleWindow(Client client, Usuari usuari, GestorBDTaller cp)
        {
            WindowStartupLocation = WindowStartupLocation.CenterOwner;
            this.client = client;
            this.usuari = usuari;
            this.cp = cp;
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            lstVehicles.ItemsSource = client.Vehicles;
            txbClient.Text = client.Nom + " " + client.Cognoms + ", DNI: " + client.Nif;
        }
        private void Item_MouseUp(object sender, MouseButtonEventArgs e)
        {
            Vehicle selectedVehicle = (Vehicle) ((FrameworkElement)sender).DataContext;

            // Creem una nova reparació amb data d'avui i el vehicle seleccionat
            Reparacio reparacio = new Reparacio(selectedVehicle.Matricula, EstatReparacio.OBERTA, DateTime.Now, 0);
            ReparacioWindow rw = new ReparacioWindow(reparacio, usuari, true, cp);
            rw.Owner = Owner;
            rw.Show();
            Close();
        }

    }
}
