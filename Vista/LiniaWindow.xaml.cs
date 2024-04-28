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
    /// Lógica de interacción para LiniaWindow.xaml
    /// </summary>
    public partial class LiniaWindow : Window
    {
        GestorBDTaller cp;
        Usuari usuari;
        Linia linia;
        bool creacio;
        bool descripcioOk = false, quantitatOk = false, preuOk = false, codiOk = false, preuUnitatOk = false, descompteOk = false;

        public LiniaWindow(Usuari usuari, Linia linia, bool creacio, GestorBDTaller cp)
        {
            WindowStartupLocation = WindowStartupLocation.CenterOwner;
            this.usuari = usuari;
            this.linia = linia;
            this.creacio = creacio;
            this.cp = cp;
            InitializeComponent();
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            btnDesar.Visibility = Visibility.Collapsed;

            if (creacio && usuari.Tipus == TipusUsuari.RECEPCIO)
            {
                // Les noves línies de recepcionistes tenen un tipus de visibilitat concret
                gridCodi.Visibility = Visibility.Collapsed;
                gridPreu.Visibility = Visibility.Collapsed;
                gridPreuUnitat.Visibility = Visibility.Collapsed;
                cbPacks.Visibility = Visibility.Collapsed;
                gridDescompte.Visibility = !creacio ? Visibility.Visible : Visibility.Collapsed;
                lblQuantitat.Content = "Quantitat d'hores:";
                cbTipus.IsEnabled = false;
                gridQuantitat.Visibility = Visibility.Collapsed;
            }
            else
            {

                List<Pack> packs = cp.obtenirPacks();
                foreach (Pack pack in packs)
                {
                    ComboBoxItem item = new ComboBoxItem();
                    item.Content = pack.Nom;
                    item.Tag = pack.Preu;
                    cbPacks.Items.Add(item);
                }

                // La resta de casos tenen una visibilitat especificada per una funció
                establirVisibilitatCamps(linia.Tipus);
                gridDescompte.Visibility = Visibility.Collapsed;
                if (!creacio)
                {
                    txbDescripcio.Text = linia.Descripcio;
                    txbQuantitat.Text = linia.Quantitat.ToString();
                    txbPreu.Text = linia.Preu.ToString();
                    txbPreuUnitat.Text = linia.PreuUnitari.ToString();
                    txbCodi.Text = linia.CodiFabricant;
                }

                // si es un recepcionista, no pot modificar cap dels camps
                if (usuari.Tipus == TipusUsuari.RECEPCIO)
                {
                    txbDescripcio.IsEnabled = false;
                    txbQuantitat.IsEnabled = false;
                    txbPreu.IsEnabled = false;
                    txbPreuUnitat.IsEnabled = false;
                    txbCodi.IsEnabled = false;
                    cbTipus.IsEnabled = false;
                    gridDescompte.Visibility = Visibility.Visible;
                    if (linia.Descompte != null)
                        txbDescompte.Text = linia.Descompte.ToString();
                    btnDesar.Visibility = Visibility.Collapsed;
                }
            }

            if (!creacio) cbTipus.IsEnabled = false;
        }

        private void cbTipus_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            if (gridCodi == null || gridPreu == null || gridPreuUnitat == null || cbPacks == null || gridQuantitat == null || lblQuantitat == null)
            {
                return;
            }

            string? tipusString = ((ComboBoxItem)cbTipus.SelectedValue).Content.ToString();
            TipusLinia tipus = tipusFromString(tipusString);
            establirVisibilitatCamps(tipus);
            calcularVisibilityDesar();
        }

        private void btnDesar_Click(object sender, RoutedEventArgs e)
        {

            if (usuari.Tipus == TipusUsuari.RECEPCIO && !creacio)
            {
                linia.Descompte = int.Parse(txbDescompte.Text);
                DialogResult = true;
                Close();
                return;
            }
            linia.Tipus = tipusFromString(((ComboBoxItem)cbTipus.SelectedValue).Content.ToString());

            if (linia.Tipus == TipusLinia.PACK)
                linia.Descripcio = (((ComboBoxItem)cbPacks.SelectedValue).Content.ToString());
            else
                linia.Descripcio = txbDescripcio.Text;

            try
            {
                linia.Quantitat = txbQuantitat.Text == "" ? null : int.Parse(txbQuantitat.Text);
                linia.Preu = txbPreu.Text == "" ? null : decimal.Parse(txbPreu.Text);
                linia.PreuUnitari = txbPreuUnitat.Text == "" ? null : decimal.Parse(txbPreuUnitat.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: camps en format incorrecte");
                return;
            }
            linia.CodiFabricant = txbCodi.Text;
            DialogResult = true;
            Close();
        }

        void establirVisibilitatCamps(TipusLinia tipus)
        {
            switch (tipus)
            {
                case TipusLinia.FEINA:
                    cbTipus.SelectedIndex = 0;
                    gridCodi.Visibility = Visibility.Collapsed;
                    gridPreu.Visibility = Visibility.Collapsed;
                    gridPreuUnitat.Visibility = Visibility.Collapsed;
                    cbPacks.Visibility = Visibility.Collapsed;
                    gridQuantitat.Visibility = Visibility.Visible;
                    lblQuantitat.Content = "Quantitat d'hores:";
                    break;
                case TipusLinia.PEÇA:
                    cbTipus.SelectedIndex = 1;
                    gridCodi.Visibility = Visibility.Visible;
                    gridPreu.Visibility = Visibility.Collapsed;
                    gridPreuUnitat.Visibility = Visibility.Visible;
                    cbPacks.Visibility = Visibility.Collapsed;
                    gridQuantitat.Visibility = Visibility.Visible;
                    lblQuantitat.Content = "Quantitat";
                    break;
                case TipusLinia.PACK:
                    cbTipus.SelectedIndex = 2;
                    gridCodi.Visibility = Visibility.Collapsed;
                    gridPreu.Visibility = Visibility.Collapsed;
                    gridPreuUnitat.Visibility = Visibility.Collapsed;
                    gridQuantitat.Visibility = Visibility.Collapsed;
                    if (usuari.Tipus == TipusUsuari.MECANIC)
                        cbPacks.Visibility = Visibility.Visible;
                    else
                        cbPacks.Visibility = Visibility.Collapsed;
                    break;
                case TipusLinia.ALTRES:
                    cbTipus.SelectedIndex = 3;
                    gridCodi.Visibility = Visibility.Collapsed;
                    gridPreu.Visibility = Visibility.Visible;
                    txbPreu.IsEnabled = true;
                    gridPreuUnitat.Visibility = Visibility.Collapsed;
                    cbPacks.Visibility = Visibility.Collapsed;
                    gridQuantitat.Visibility = Visibility.Collapsed;
                    break;
            }
        }

        TipusLinia tipusFromString(string tipusString)
        {
            switch (tipusString)
            {
                case "Feina":
                    return TipusLinia.FEINA;
                case "Peça":
                    return TipusLinia.PEÇA;
                case "Pack":
                    return TipusLinia.PACK;
                case "Altres":
                    return TipusLinia.ALTRES;
                default:
                    return TipusLinia.FEINA;
            }
        }

        private void btnCancelar_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private void txbDescripcio_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (txbDescripcio.Text.Length >= 3)
            {
                descripcioOk = true;
                txbDescripcio.Background = Brushes.White;
            }
            else
            {
                descripcioOk = false;
                txbDescripcio.Background = Brushes.Red;
            }
            calcularVisibilityDesar();
        }

        private void cbPacks_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbPacks.SelectedIndex == -1)
            {
                return;
            }

            try
            {
                string descripcio = cbPacks.Text;
                decimal preu = decimal.Parse(((ComboBoxItem)cbPacks.SelectedItem).Tag.ToString());

                gridPreu.Visibility = Visibility.Visible;
                txbDescripcio.Text = descripcio;
                txbPreu.Text = preu.ToString();
                txbPreu.IsEnabled = false;
                btnDesar.Visibility = Visibility.Visible;
            }
            catch (Exception ex)
            {
                return;
            }

        }

        private void txbQuantitat_TextChanged(object sender, TextChangedEventArgs e)
        {

            // Comprovem que sigui un numero enter
            int quantitat;
            try
            {
                quantitat = int.Parse(txbQuantitat.Text);
            }
            catch (Exception ex)
            {
                quantitatOk = false;
                txbQuantitat.Background = Brushes.Red;
                calcularVisibilityDesar();
                return;
            }

            // Comprovem que sigui enter positiu més gran o igual que 1
            if (quantitat >= 1)
            {
                quantitatOk = true;
                txbQuantitat.Background = Brushes.White;
            }
            else
            {
                quantitatOk = false;
                txbQuantitat.Background = Brushes.Red;
            }
            calcularVisibilityDesar();
        }

        private void txbPreuUnitat_TextChanged(object sender, TextChangedEventArgs e)
        {
            // Comprovem que sigui un numero decimal
            decimal preu;
            try
            {
                preu = decimal.Parse(txbPreuUnitat.Text);
            }
            catch (Exception ex)
            {
                preuUnitatOk = false;
                txbPreuUnitat.Background = Brushes.Red;
                calcularVisibilityDesar();
                return;
            }

            if (txbPreuUnitat.Text.ToCharArray().Count(c => c == ',' || c == '.') > 1)
            {
                preuUnitatOk = false;
                txbPreuUnitat.Background = Brushes.Red;
                calcularVisibilityDesar();
                return;
            }

            // Comprovem que el preu sigui major que 0 i només tingui 2 decimals
            if (preu > 0)
            {

                if (txbPreuUnitat.Text.Contains(","))
                {
                    string[] parts = txbPreuUnitat.Text.Split(',');
                    if (parts[1].Length > 2)
                    {
                        txbPreuUnitat.Text = parts[0] + "," + parts[1].Substring(0, 2);
                        txbPreuUnitat.CaretIndex = txbPreuUnitat.Text.Length;
                    }
                }
                else if (txbPreuUnitat.Text.Contains("."))
                {
                    string[] parts = txbPreuUnitat.Text.Split('.');
                    if (parts[1].Length > 2)
                    {
                        txbPreuUnitat.Text = parts[0] + "." + parts[1].Substring(0, 2);
                        txbPreuUnitat.CaretIndex = txbPreuUnitat.Text.Length;
                    }
                }

                preuUnitatOk = true;
                txbPreuUnitat.Background = Brushes.White;
            }
            else
            {
                preuUnitatOk = false;
                txbPreuUnitat.Background = Brushes.Red;
            }
            calcularVisibilityDesar();
        }

        private void txbPreu_TextChanged(object sender, TextChangedEventArgs e)
        {
            // Comprovem que sigui un numero decimal
            decimal preu;
            try
            {
                preu = decimal.Parse(txbPreu.Text);
            }
            catch (Exception ex)
            {
                preuOk = false;
                txbPreu.Background = Brushes.Red;
                calcularVisibilityDesar();
                return;
            }

            if (txbPreu.Text.ToCharArray().Count(c => c == ',' || c == '.') > 1)
            {
                preuOk = false;
                txbPreu.Background = Brushes.Red;
                calcularVisibilityDesar();
                return;
            }

            // Comprovem que el preu sigui major que 0 i només tingui 2 decimals
            if (preu > 0)
            {

                if (txbPreu.Text.Contains(","))
                {
                    string[] parts = txbPreu.Text.Split(',');
                    if (parts[1].Length > 2)
                    {
                        txbPreu.Text = parts[0] + "," + parts[1].Substring(0, 2);
                        txbPreu.CaretIndex = txbPreu.Text.Length;
                    }
                }
                else if (txbPreu.Text.Contains("."))
                {
                    string[] parts = txbPreu.Text.Split('.');
                    if (parts[1].Length > 2)
                    {
                        txbPreu.Text = parts[0] + "." + parts[1].Substring(0, 2);
                        txbPreu.CaretIndex = txbPreu.Text.Length;
                    }
                }

                preuOk = true;
                txbPreu.Background = Brushes.White;
            }
            else
            {
                preuOk = false;
                txbPreu.Background = Brushes.Red;
            }
            calcularVisibilityDesar();
        }

        private void txbCodi_TextChanged(object sender, TextChangedEventArgs e)
        {
            // Comprovem que sigui una cadena de text més gran que 2 caracters
            if (txbCodi.Text.Length > 2)
            {
                codiOk = true;
                txbCodi.Background = Brushes.White;
            }
            else
            {
                codiOk = false;
                txbCodi.Background = Brushes.Red;
            }
            calcularVisibilityDesar();
        }
        private void txbDescompte_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                int descompte = int.Parse(txbDescompte.Text);

                if (descompte >= 100 || descompte <= 0)
                {
                    throw new Exception();
                }

                txbDescompte.Background = Brushes.White;
                btnDesar.Visibility = Visibility.Visible;
            }
            catch (Exception ex)
            {
                txbDescompte.Background = Brushes.Red;
                btnDesar.Visibility = Visibility.Collapsed;
                return;
            }
        }

        void calcularVisibilityDesar()
        {
            try
            {
                bool esPotGuardar;

                string? tipusString = ((ComboBoxItem)cbTipus.SelectedValue).Content.ToString();
                TipusLinia tipus = tipusFromString(tipusString);
                switch (tipus)
                {
                    case TipusLinia.FEINA:
                        if (txbDescripcio.Text != linia.Descripcio || int.Parse(txbQuantitat.Text) != linia.Quantitat)
                        {
                            if (usuari.Tipus == TipusUsuari.RECEPCIO)
                                esPotGuardar = descripcioOk;
                            else
                                esPotGuardar = descripcioOk && quantitatOk;
                        }
                        else
                            esPotGuardar = false;
                        break;
                    case TipusLinia.PEÇA:
                        if (txbDescripcio.Text != linia.Descripcio || int.Parse(txbQuantitat.Text) != linia.Quantitat ||
                            decimal.Parse(txbPreuUnitat.Text) != linia.PreuUnitari || txbCodi.Text != linia.CodiFabricant)
                            esPotGuardar = descripcioOk && quantitatOk && codiOk && preuUnitatOk;
                        else
                            esPotGuardar = false;
                        break;
                    case TipusLinia.PACK:
                        esPotGuardar = false;
                        break;
                    case TipusLinia.ALTRES:
                        if (txbDescripcio.Text != linia.Descripcio || decimal.Parse(txbPreu.Text) != linia.Preu)
                            esPotGuardar = descripcioOk && preuOk;
                        else
                            esPotGuardar = false;
                        break;
                    default:
                        esPotGuardar = false;
                        break;
                }

                if (esPotGuardar)
                {
                    btnDesar.Visibility = Visibility.Visible;
                }
                else
                {
                    btnDesar.Visibility = Visibility.Collapsed;
                }
            }
            catch (Exception ex)
            {
                btnDesar.Visibility = Visibility.Collapsed;
            }
        }
    }
}
