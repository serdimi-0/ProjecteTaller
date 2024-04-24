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
            gridCodi.Visibility = Visibility.Collapsed;
            gridPreu.Visibility = Visibility.Collapsed;
            gridPreuUnitat.Visibility = Visibility.Collapsed;
            cbDescripcio.Visibility = Visibility.Collapsed;
            gridQuantitat.Visibility = Visibility.Visible;
            lblQuantitat.Content = "Quantitat d'hores:";


            if (usuari.Tipus == TipusUsuari.RECEPCIO)
            {
                cbTipus.IsEnabled = false;
                gridQuantitat.Visibility = Visibility.Collapsed;
            }

            if (!creacio) cbTipus.IsEnabled = false;
        }

        private void cbTipus_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            if(gridCodi == null || gridPreu == null || gridPreuUnitat == null || cbDescripcio == null || gridQuantitat == null || lblQuantitat == null)
            {
                return;
            }

            string? tipus = ((ComboBoxItem) cbTipus.SelectedValue).Content.ToString();
            switch (tipus)
            {
                case "Feina":
                    gridCodi.Visibility = Visibility.Collapsed;
                    gridPreu.Visibility = Visibility.Collapsed;
                    gridPreuUnitat.Visibility = Visibility.Collapsed;
                    cbDescripcio.Visibility = Visibility.Collapsed;
                    gridQuantitat.Visibility = Visibility.Collapsed;
                    lblQuantitat.Content = "Quantitat d'hores:";
                    break;
                case "Peça":
                    gridCodi.Visibility = Visibility.Visible;
                    gridPreu.Visibility = Visibility.Collapsed;
                    gridPreuUnitat.Visibility = Visibility.Visible;
                    cbDescripcio.Visibility = Visibility.Collapsed;
                    gridQuantitat.Visibility = Visibility.Visible;
                    lblQuantitat.Content = "Quantitat";
                    break;
                case "Pack":
                    gridCodi.Visibility = Visibility.Collapsed;
                    gridPreu.Visibility = Visibility.Collapsed;
                    gridPreuUnitat.Visibility = Visibility.Collapsed;
                    cbDescripcio.Visibility = Visibility.Visible;
                    gridQuantitat.Visibility = Visibility.Collapsed;
                    break;
                case "Altres":
                    gridCodi.Visibility = Visibility.Collapsed;
                    gridPreu.Visibility = Visibility.Visible;
                    gridPreuUnitat.Visibility = Visibility.Collapsed;
                    cbDescripcio.Visibility = Visibility.Collapsed;
                    gridQuantitat.Visibility = Visibility.Collapsed;
                    break;
            }
        }

        private void btnDesar_Click(object sender, RoutedEventArgs e)
        {
            linia.Descripcio = txbDescripcio.Text;
            DialogResult = true;
            Close();
        }

    }
}
