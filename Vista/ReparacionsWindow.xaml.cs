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
    /// Lógica de interacción para ReparacionsWindow.xaml
    /// </summary>
    public partial class ReparacionsWindow : Window
    {
        Usuari usuari;
        GestorBDTaller cp;
        List<Reparacio> reparacions;

        public ReparacionsWindow(Usuari usuari, GestorBDTaller cp)
        {
            this.usuari = usuari;
            this.cp = cp;
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            lblUser.Content = "Usuari: " + usuari.Login;

            reparacions = cp.obtenirReparacions();
            foreach (Reparacio r in reparacions)
            {
                r.Model = cp.obtenirVehicle(r.VehicleId).Model;
            }

            switch (usuari.Tipus)
            {
                case TipusUsuari.MECANIC:
                    opcionsMostrar.Visibility = Visibility.Collapsed;
                    lsvReparacions.ItemsSource = reparacions.Where(r => r.Estat == EstatReparacio.OBERTA);
                    break;
                case TipusUsuari.ADMIN:
                    cbMostrar.SelectedIndex = 0;
                    lsvReparacions.ItemsSource = reparacions;
                    break;
                case TipusUsuari.RECEPCIO:
                    lsvReparacions.ItemsSource = reparacions.Where(r => r.Estat == EstatReparacio.TANCADA);
                    break;
            }

        }

        private void updateList()
        {
            /* El contingut de la llista depen de tres variables:
             1- el contingut del textbox de cerca
             2- la opció d'ordenació seleccionada
             3- el filtre seleccionat*/

            string cerca = txtSearch.Text;
            int opcioOrdre = cbOrdre.SelectedIndex;
            int opcioMostrar = cbMostrar.SelectedIndex;

            IEnumerable<Reparacio> reparacionsFiltrades = reparacions;
            reparacionsFiltrades = reparacionsFiltrades.Where(r => r.Model.Contains(cerca));
            

        }

        private void txtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            updateList();
        }

        private void cbOrdre_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            updateList();
        }

        private void cbMostrar_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            updateList();
        }
    }
}
