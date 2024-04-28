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
    /// Lógica de interacción para FacturacioWindow.xaml
    /// </summary>
    public partial class FacturacioWindow : Window
    {
        Reparacio reparacio;
        public Factura factura;
        decimal preuMaObra;
        List<Linia> liniesCopy;

        public FacturacioWindow(Reparacio reparacio)
        {
            this.reparacio = reparacio;
            factura = new Factura(reparacio.Id);
            liniesCopy = new List<Linia>(reparacio.Linies);
            WindowStartupLocation = WindowStartupLocation.CenterOwner;
            InitializeComponent();
        }

        private void txbPreuMa_TextChanged(object sender, TextChangedEventArgs e)
        {
            calcularCamps();
        }

        private void btnFacturar_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Estàs segur que vols facturar aquesta reparació?", "Facturar", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                reparacio.Linies = liniesCopy;
                DialogResult = true;
                Close();
            }
        }

        private void calcularCamps()
        {
            if (txbPreuMa.Text != "" && decimal.TryParse(txbPreuMa.Text, out decimal x) && x > 0)
            {
                btnFacturar.Visibility = Visibility.Visible;
                preuMaObra = decimal.Parse(txbPreuMa.Text);

                DateTime data = DateTime.Now;
                decimal subtotal = calcularSubtotal();
                int tipusIva = int.Parse(((ComboBoxItem)cbIva.SelectedItem).Tag.ToString());
                decimal iva = subtotal * tipusIva / 100;
                decimal total = subtotal + iva;


                txbData.Text = data.ToString("dd/MM/yyyy HH:MM");
                txbSubtotal.Text = subtotal.ToString("0.00€");
                txbIva.Text = iva.ToString("0.00€");
                txbTotal.Text = total.ToString("0.00€");

                dtgLinies.ItemsSource = liniesCopy.Select(l => new { l.Descripcio, l.Preu, l.Descompte, l.Import });
                foreach (DataGridColumn column in dtgLinies.Columns)
                {
                    column.Width = new DataGridLength(1, DataGridLengthUnitType.Star);
                }

                factura.Data = data;
                factura.Subtotal = subtotal;
                factura.Iva = iva;
                factura.Total = total;
                factura.PreuMaObra = preuMaObra;
                factura.TipusIva = tipusIva;
            }
            else
            {
                btnFacturar.Visibility = Visibility.Collapsed;
                txbData.Text = "";
                txbSubtotal.Text = "";
                txbIva.Text = "";
                txbTotal.Text = "";
            }
        }

        private decimal calcularSubtotal()
        {
            decimal total = 0;

            foreach (Linia linia in liniesCopy)
            {
                switch (linia.Tipus)
                {
                    case TipusLinia.FEINA:
                        linia.Preu = preuMaObra * linia.Quantitat;
                        break;
                    case TipusLinia.PEÇA:
                        linia.Preu = linia.PreuUnitari * linia.Quantitat;
                        break;
                    // La resta de tipus de linia ja tenen el preu definit
                    case TipusLinia.PACK:
                    case TipusLinia.ALTRES:
                        break;
                }

                linia.Descompte = linia.Descompte ?? 0;

                linia.Import = linia.Preu - linia.Preu * linia.Descompte / 100;
                total += linia.Import ?? 0;
            }

            return total;

        }

        private void cbIva_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbIva != null && txbPreuMa != null && cbIva.SelectedItem != null)
            {
                calcularCamps();
            }
        }
    }
}
