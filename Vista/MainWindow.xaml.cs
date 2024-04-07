using CapaMongoDB;
using InterficiePersistencia;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Configuration;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Vista
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                GestorBDTallerMongoDB cp = new GestorBDTallerMongoDB(ConfigurationManager.AppSettings);
            }
            catch (GestorBDTallerException)
            {
                MessageBox.Show("No s'ha pogut accedir a la base de dades");
            }
        }
    }
}
