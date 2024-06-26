﻿using InterficiePersistencia;
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
        bool reparacionsCarregades = false;
        string cercaFiltre, ordreFiltre;


        public ReparacionsWindow(Usuari usuari, GestorBDTaller cp)
        {
            WindowState = WindowState.Maximized;
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
            reparacionsCarregades = true;

            switch (usuari.Tipus)
            {
                case TipusUsuari.MECANIC:
                    cbMostrar.SelectedIndex = 1;
                    opcionsMostrar.Visibility = Visibility.Collapsed;
                    lsvReparacions.ItemsSource = reparacions.Where(r => r.Estat == EstatReparacio.OBERTA).OrderBy(r => r.Data);
                    break;
                case TipusUsuari.ADMIN:
                    cbMostrar.SelectedIndex = 0;
                    lsvReparacions.ItemsSource = reparacions.OrderBy(r => r.Data);
                    break;
                case TipusUsuari.RECEPCIO:
                    lsvReparacions.ItemsSource = reparacions.Where(r => r.Estat == EstatReparacio.TANCADA).OrderBy(r => r.Data);
                    break;
            }

            btnAfegir.Visibility = usuari.Tipus == TipusUsuari.RECEPCIO ? Visibility.Visible : Visibility.Collapsed;
        }

        private void updateList()
        {
            ordreFiltre = cbOrdre.Text;
            cercaFiltre = txtSearch.Text;

            IEnumerable<Reparacio> reparacionsFiltrades = reparacions;
            switch (cbMostrar.SelectedIndex)
            {
                case 0:
                    break;
                case 1:
                    reparacionsFiltrades = reparacionsFiltrades.Where(r => r.Estat == EstatReparacio.OBERTA);
                    break;
                case 2:
                    reparacionsFiltrades = reparacionsFiltrades.Where(r => r.Estat == EstatReparacio.TANCADA);
                    break;
                case 3:
                    reparacionsFiltrades = reparacionsFiltrades.Where(r => r.Estat == EstatReparacio.FACTURADA);
                    break;
            }

            if (cercaFiltre != "")
            {
                reparacionsFiltrades = reparacionsFiltrades.Where(r => r.VehicleId.ToLower().Contains(cercaFiltre.ToLower()) || r.Model.ToLower().Contains(cercaFiltre.ToLower()));
            }

            if (ordreFiltre.Contains("asc"))
                reparacionsFiltrades = reparacionsFiltrades.OrderBy(r => r.Data);
            else
                reparacionsFiltrades = reparacionsFiltrades.OrderByDescending(r => r.Data);

            lsvReparacions.ItemsSource = reparacionsFiltrades;
        }

        private void txtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (reparacionsCarregades)
                updateList();
        }

        private void cbOrdre_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (reparacionsCarregades)
                updateList();
        }

        private void lsvReparacions_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Reparacio reparacio = (Reparacio)lsvReparacions.SelectedItem;
            if (reparacio == null)
                return;
            ReparacioWindow reparacioWindow = new ReparacioWindow(reparacio, usuari, false, cp);
            reparacioWindow.Owner = this;
            reparacioWindow.ShowDialog();
            lsvReparacions.UnselectAll();
            reparacions = cp.obtenirReparacions();
            foreach (Reparacio r in reparacions)
            {
                r.Model = cp.obtenirVehicle(r.VehicleId).Model;
            }
            updateList();
        }

        private void btnAfegir_Click(object sender, RoutedEventArgs e)
        {
            SeleccioClientWindow seleccioClientWindow = new SeleccioClientWindow(usuari, cp);
            seleccioClientWindow.Owner = this;
            seleccioClientWindow.ShowDialog();

            if(seleccioClientWindow.DialogResult == false)
                return;

            SeleccioVehicleWindow seleccioVehicleWindow = new SeleccioVehicleWindow(seleccioClientWindow.selectedClient, usuari, cp);
            seleccioVehicleWindow.Owner = this;
            seleccioVehicleWindow.ShowDialog();

            if (seleccioVehicleWindow.DialogResult == false)
                return;

            ReparacioWindow reparacioWindow = new ReparacioWindow(seleccioVehicleWindow.novaReparacio, usuari, true, cp);
            reparacioWindow.Owner = this;
            reparacioWindow.ShowDialog();

            if (seleccioVehicleWindow.DialogResult == false)
                return;

            reparacions = cp.obtenirReparacions();
            foreach (Reparacio r in reparacions)
            {
                r.Model = cp.obtenirVehicle(r.VehicleId).Model;
            }
            lsvReparacions.Items.Refresh();
            updateList();
        }

        private void cbMostrar_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (reparacionsCarregades)
                updateList();
        }
    }
}
