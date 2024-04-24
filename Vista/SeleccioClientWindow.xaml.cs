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
    /// Lógica de interacción para CrearReparacioWindow.xaml
    /// </summary>
    public partial class SeleccioClientWindow : Window
    {
        Usuari usuari;
        GestorBDTaller cp;
        List<Client> clients;
        public Client selectedClient;

        public SeleccioClientWindow(Usuari usuari, GestorBDTaller cp)
        {
            WindowStartupLocation = WindowStartupLocation.CenterOwner;
            this.usuari = usuari;
            this.cp = cp;
            clients = cp.obtenirClients();
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            dtgClients.ItemsSource = clients;
            dtgClients.Columns[0].Visibility = Visibility.Collapsed;
            dtgClients.Columns[6].Visibility = Visibility.Collapsed;

        }

        private void btnSeleccio_Click(object sender, RoutedEventArgs e)
        {
            if (dtgClients.SelectedItem == null)
            {
                MessageBox.Show("Has de seleccionar un client!");
                return;
            }

            Client client = (Client)dtgClients.SelectedItem;
            cp.obtenirVehicles(client);
            selectedClient = client;
            DialogResult = true;
            Close();

        }

        private void txbCerca_TextChanged(object sender, TextChangedEventArgs e)
        {
            // search for clients by any of the fields
            string cerca = txbCerca.Text;
            dtgClients.ItemsSource = clients.Where(c => c.Nom.ToLower().Contains(cerca.ToLower()) ||
                                                        c.Cognoms.ToLower().Contains(cerca.ToLower()) ||
                                                        c.Nif.ToLower().Contains(cerca.ToLower()) ||
                                                        c.Telefon.ToLower().Contains(cerca.ToLower()) ||
                                                        c.Direccio.ToLower().Contains(cerca.ToLower()));
            if(dtgClients.Items.Count == 0)
            {
                return;
            }
            dtgClients.Columns[0].Visibility = Visibility.Collapsed;
            dtgClients.Columns[6].Visibility = Visibility.Collapsed;
        }
    }
}
