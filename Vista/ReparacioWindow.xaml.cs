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
                    btnAfegir.Visibility = Visibility.Collapsed;
                    btnFacturar.Visibility = reparacio.Factura == null ? Visibility.Visible : Visibility.Collapsed;
                    btnImprimir.Visibility = reparacio.Factura == null ? Visibility.Collapsed : Visibility.Visible;
                    btnPagar.Visibility = reparacio.FacturaPagada ? Visibility.Collapsed : Visibility.Visible;
                }
            }

            btnDesar.Visibility = creacio ? Visibility.Visible : Visibility.Collapsed;
        }

        private void btnAfegir_Click(object sender, RoutedEventArgs e)
        {
            Linia linia = new Linia();
            linia.Numero = reparacio.Linies.Count + 1;

            LiniaWindow lw = new LiniaWindow(usuari, linia, true, cp);
            lw.Owner = this;
            lw.ShowDialog();
            if (lw.DialogResult == true)
            {
                reparacio.Linies.Add(linia);
                lsvLinies.Items.Refresh();
                btnDesar.Visibility = Visibility.Visible;
                btnTancar.Visibility = Visibility.Collapsed;
            }

        }

        private void btnDesar_Click(object sender, RoutedEventArgs e)
        {
            if (creacio)
            {
                MessageBoxResult result = MessageBox.Show("Estàs segur que vols crear aquesta reparació? No podràs modificar-la més endevant.", "Creació de reparació", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.No) return;

                cp.insertarReparacio(reparacio);

            }
            else
            {
                MessageBoxResult result = MessageBox.Show("Estàs segur que vols modificar aquesta reparació?", "Modificació de reparació", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.No) return;

                cp.modificarReparacio(reparacio);
            }
            btnDesar.Visibility = Visibility.Collapsed;
            btnTancar.Visibility = usuari.Tipus == TipusUsuari.MECANIC ? Visibility.Visible : Visibility.Collapsed;
            btnFacturar.Visibility = usuari.Tipus == TipusUsuari.RECEPCIO && reparacio.Estat == EstatReparacio.TANCADA ? Visibility.Visible : Visibility.Collapsed;
        }

        private void btnEliminar_Click(object sender, RoutedEventArgs e)
        {
            reparacio.Linies.Remove((Linia)lsvLinies.SelectedItem);
            lsvLinies.Items.Refresh();
            btnDesar.Visibility = Visibility.Visible;
            btnTancar.Visibility = Visibility.Collapsed;
        }

        private void btnModificar_Click(object sender, RoutedEventArgs e)
        {
            Linia linia = (Linia)lsvLinies.SelectedItem;
            LiniaWindow lw = new LiniaWindow(usuari, linia, false, cp);
            lw.Owner = this;
            lw.ShowDialog();
            if (lw.DialogResult == true)
            {
                lsvLinies.Items.Refresh();
                btnDesar.Visibility = Visibility.Visible;
                btnTancar.Visibility = Visibility.Collapsed;
                btnFacturar.Visibility = Visibility.Collapsed;
                btnPagar.Visibility = Visibility.Collapsed;
            }
        }

        private void lsvLinies_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lsvLinies.SelectedItem == null)
            {
                btnEliminar.Visibility = Visibility.Collapsed;
                btnModificar.Visibility = Visibility.Collapsed;
                return;
            }
            btnEliminar.Visibility = usuari.Tipus == TipusUsuari.MECANIC ? Visibility.Visible : Visibility.Collapsed;
            btnModificar.Visibility = usuari.Tipus == TipusUsuari.RECEPCIO && reparacio.Estat == EstatReparacio.TANCADA ||
                                      usuari.Tipus == TipusUsuari.MECANIC ? Visibility.Visible : Visibility.Collapsed;
        }

        private void btnTancar_Click(object sender, RoutedEventArgs e)
        {

            foreach (Linia linia in reparacio.Linies)
            {

                // Comprovació de que tots els camps siguin correctes
                if (linia.Descripcio == "" || linia.Descripcio.Length < 3)
                {
                    MessageBox.Show("Alguna de les línies conté una descripció incorrecta", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                if (linia.Tipus == TipusLinia.PEÇA && linia.PreuUnitari <= 0)
                {
                    MessageBox.Show("Alguna de les línies de tipus \"peça\" té un preu unitari incorrecte", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                if (linia.Tipus == TipusLinia.PEÇA && (linia.CodiFabricant == null || linia.CodiFabricant.Length < 3))
                {
                    MessageBox.Show("Alguna de les línies de tipus \"peça\" té un codi de fabricant incorrecte", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                if (linia.Tipus == TipusLinia.ALTRES && linia.Preu <= 0)
                {
                    MessageBox.Show("Alguna de les línies de tipus \"altres\" té un preu incorrecte", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                if (linia.Tipus != TipusLinia.ALTRES && linia.Tipus != TipusLinia.PACK && (linia.Quantitat == null || linia.Quantitat <= 0))
                {
                    MessageBox.Show("Alguna de les línies té una quantitat incorrecta", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                // Càlcul del preu de la línia en cas de ser peça
                if (linia.Tipus == TipusLinia.PEÇA)
                {
                    linia.Preu = linia.PreuUnitari * linia.Quantitat;
                    break;
                }

                // Les línies de tipus "pack" i "altres" ja tenen el preu calculat
                // Les línies de tipus "feina" només ténen preu una vegada facturades
            }


            MessageBoxResult result = MessageBox.Show("Estàs segur que vols tancar aquesta reparació?", "Tancament de reparació", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.No) return;
            cp.modificarReparacio(reparacio);
            cp.canviarEstatReparacio(reparacio, EstatReparacio.TANCADA);
            Close();
        }

        private void btnRebutjar_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Estàs segur que vols rebutjar aquesta reparació?", "Rebutjar reparació", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.No) return;
            cp.canviarEstatReparacio(reparacio, EstatReparacio.REBUTJADA);
            Close();
        }
    }
}
