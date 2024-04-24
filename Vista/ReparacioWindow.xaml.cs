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
        Usuari usuari;
        GestorBDTaller cp;
        bool creacio;

        public ReparacioWindow(Reparacio reparacio, Usuari usuari, bool creacio, GestorBDTaller cp)
        {
            this.reparacio = reparacio;
            this.creacio = creacio;
            this.usuari = usuari;
            this.cp = cp;

            if (!creacio)
                cp.obtenirLinies(reparacio);
            else
                reparacio.Linies = new List<Linia>();
            WindowStartupLocation = WindowStartupLocation.CenterOwner;
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            txbEstat.Text = creacio ? "Creació" : reparacio.EstatString;
            txbData.Text = reparacio.DataString;
            txbMatricula.Text = reparacio.VehicleId;
            txbModel.Text = reparacio.Model;
            lsvLinies.ItemsSource = reparacio.Linies;
            btnDesar.Visibility = creacio ? Visibility.Visible : Visibility.Collapsed;

            if (usuari.Tipus == TipusUsuari.MECANIC)
            {
                btnTancar.Visibility = Visibility.Visible;
                btnRebutjar.Visibility = Visibility.Visible;
                btnFacturar.Visibility = Visibility.Collapsed;
                btnImprimir.Visibility = Visibility.Collapsed;
                btnPagar.Visibility = Visibility.Collapsed;
            }
            else if (usuari.Tipus == TipusUsuari.RECEPCIO)
            {
                btnTancar.Visibility = Visibility.Collapsed;
                btnRebutjar.Visibility = Visibility.Collapsed;

                if (reparacio.Estat == EstatReparacio.OBERTA)
                {
                    btnAfegir.Visibility = creacio ? Visibility.Visible : Visibility.Collapsed;
                    btnFacturar.Visibility = Visibility.Collapsed;
                    btnImprimir.Visibility = Visibility.Collapsed;
                    btnPagar.Visibility = Visibility.Collapsed;
                }
                else
                {
                    btnAfegir.Visibility = reparacio.Factura == null ? Visibility.Visible : Visibility.Collapsed;
                    btnFacturar.Visibility = reparacio.Factura == null ? Visibility.Visible : Visibility.Collapsed;
                    btnImprimir.Visibility = reparacio.Factura == null ? Visibility.Collapsed : Visibility.Visible;
                    btnPagar.Visibility = reparacio.FacturaPagada ? Visibility.Collapsed : Visibility.Visible;
                }
            }
        }

        private void btnAfegir_Click(object sender, RoutedEventArgs e)
        {
            Linia linia = new Linia();
            linia.Numero = reparacio.Linies.Count + 1;

            LiniaWindow lw = new LiniaWindow(usuari, linia, true, cp);
            lw.Owner = this;
            lw.ShowDialog();
            // get linia from LiniaWindow
            if (lw.DialogResult == true)
            {
                reparacio.Linies.Add(linia);
                lsvLinies.Items.Refresh();
            }

        }

        private void btnDesar_Click(object sender, RoutedEventArgs e)
        {
            if(creacio)
            {
                MessageBoxResult result = MessageBox.Show("Estàs segur que vols crear la reparació?", "Creació de reparació", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.No)
                    return;

                cp.insertarReparacio(reparacio);

            }
            /*else
            {
                cp.actualitzarReparacio(reparacio);
            }*/
            /*DialogResult = true;*/
            Close();
        }
    }
}
