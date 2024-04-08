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

        public ReparacionsWindow(Usuari usuari, GestorBDTaller cp)
        {
            this.usuari = usuari;
            this.cp = cp;
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            lblUser.Content = usuari.Login;
            List<Reparacio>? reparacions = cp.obtenirReparacions();
            if (reparacions == null)
            {
                MessageBox.Show("No s'han pogut obtenir reparacions");
                return;
            }
            foreach (Reparacio r in reparacions)
            {
                lsvReparacions.Text += r + "\n";
            }
        }
    }
}
