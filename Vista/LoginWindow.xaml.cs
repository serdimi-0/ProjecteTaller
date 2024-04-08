﻿using InterficiePersistencia;
using Model;
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
    public partial class LoginWindow : Window
    {
        GestorBDTaller? cp;

        public LoginWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            try
            {

                string? className = ConfigurationManager.AppSettings["classe_capa"];
                if (className == null)
                    throw new GestorBDTallerException();

                Type? capaType = Type.GetType(className);
                if (capaType == null)
                    throw new GestorBDTallerException();

                cp = (GestorBDTaller?)Activator.CreateInstance(capaType, new object[] { ConfigurationManager.AppSettings });
                if (cp == null)
                    throw new GestorBDTallerException();

            }
            catch (GestorBDTallerException)
            {
                MessageBox.Show("No s'ha pogut accedir a la base de dades");
                Application.Current.Shutdown();
                return;
            }
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            string login = txbLogin.Text;
            string password = txbPassword.Password;

            if (cp != null)
            {
                Usuari? usuari = cp.verificarUsuari(login, password);
                if (usuari == null)
                {
                    MessageBox.Show("Usuari o contrasenya incorrectes");
                }
                else
                {
                    // navigate to ReparacionsWindow
                    ReparacionsWindow reparacionsWindow = new ReparacionsWindow(usuari, cp);
                    reparacionsWindow.Show();
                    this.Close();
                }
            }
        }
    }
}