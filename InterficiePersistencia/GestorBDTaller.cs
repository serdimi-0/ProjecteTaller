﻿using Model;

namespace InterficiePersistencia
{
    public interface GestorBDTaller
    {

        bool connexioEstablerta();

        /// <summary>
        /// Mètode per verificar si un usuari existeix a la base de dades i si la contrasenya és correcta
        /// </summary>
        /// <param name="username">Nom d'usuari</param>
        /// <param name="password">Contrasenya de l'usuari</param>
        /// <returns></returns>
        Usuari? verificarUsuari(String username, String password);

        List<Reparacio>? obtenirReparacions();

        Vehicle obtenirVehicle(string matricula);

        Reparacio obtenirLinies(Reparacio reparacio);

        List<Client> obtenirClients();

        Client obtenirVehicles(Client client);

        bool insertarReparacio(Reparacio reparacio);

        bool modificarReparacio(Reparacio reparacio);

        bool canviarEstatReparacio(Reparacio reparacio, EstatReparacio estat);

        List<Pack> obtenirPacks();

        bool insertarFactura(Factura factura);

        /// <summary>
        /// Tanca la capa de persistència
        /// </summary>
        void tancarCapa();

        /// <summary>
        /// Mètode per fer commit dels canvis a la base de dades
        /// </summary>
        void confirmarCanvis();

        /// <summary>
        /// Mètode per desfer els canvis (rollback) a la base de dades
        /// </summary>
        void desferCanvis();
    }
}
