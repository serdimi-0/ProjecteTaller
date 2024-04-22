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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Vista
{
    /// <summary>
    /// Lógica de interacción para LiniaUC.xaml
    /// </summary>
    public partial class LiniaUC : UserControl
    {
        public LiniaUC()
        {
            InitializeComponent();
        }



        public Linia myLinia
        {
            get { return (Linia)GetValue(myLiniaProperty); }
            set { SetValue(myLiniaProperty, value); }
        }

        // Using a DependencyProperty as the backing store for myLinia.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty myLiniaProperty =
            DependencyProperty.Register("myLinia", typeof(Linia), typeof(LiniaUC), new PropertyMetadata(null));

        private void liniaUcRoot_Loaded(object sender, RoutedEventArgs e)
        {
            switch (myLinia.Tipus)
            {
                case TipusLinia.FEINA:
                    lblQuantitat.Content = "Hores:";
                    quantitatContainer.Visibility = Visibility.Visible;
                    codiFabricantContainer.Visibility = Visibility.Collapsed;
                    preuUnitariContainer.Visibility = Visibility.Collapsed;
                    break;
                case TipusLinia.PEÇA:
                    quantitatContainer.Visibility = Visibility.Visible;
                    codiFabricantContainer.Visibility = Visibility.Visible;
                    preuUnitariContainer.Visibility = Visibility.Visible;
                    break;
                case TipusLinia.PACK:
                    quantitatContainer.Visibility = Visibility.Collapsed;
                    codiFabricantContainer.Visibility = Visibility.Collapsed;
                    preuUnitariContainer.Visibility = Visibility.Collapsed;
                    break;
                default:
                    quantitatContainer.Visibility = Visibility.Collapsed;
                    codiFabricantContainer.Visibility = Visibility.Collapsed;
                    preuUnitariContainer.Visibility = Visibility.Collapsed;
                    break;
            }
        }
    }
}
