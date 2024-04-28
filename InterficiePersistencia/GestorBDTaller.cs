using Model;

namespace InterficiePersistencia
{
    public interface GestorBDTaller
    {
        /// <summary>
        /// Mètode per comprovar si la connexió amb la base de dades s'ha establert correctament
        /// </summary>
        /// <returns>True si la connexió s'ha establit correctament, false si no.</returns>
        bool connexioEstablerta();

        /// <summary>
        /// Mètode per verificar si un usuari existeix a la base de dades i si la contrasenya és correcta
        /// </summary>
        /// <param name="username">Nom d'usuari</param>
        /// <param name="password">Contrasenya de l'usuari</param>
        /// <returns>L'usuari recuperat</returns>
        Usuari? verificarUsuari(String username, String password);

        /// <summary>
        /// Mètode per obtenir totes les reparacions de la base de dades
        /// </summary>
        /// <returns>Les reparacions obtenides</returns>
        List<Reparacio>? obtenirReparacions();

        /// <summary>
        /// Mètode per obtenir el vehicle associat a una matrícula
        /// </summary>
        /// <param name="matricula">La matrícula del vehicle</param>
        /// <returns>El vehicle recuperat</returns>
        Vehicle obtenirVehicle(string matricula);

        /// <summary>
        /// Mètode per obtenir les linies d'una reparació
        /// </summary>
        /// <param name="reparacio">Reparació a omplir.</param>
        /// <returns>La reparació passada per paràmetre amb les línies omplertes</returns>
        Reparacio obtenirLinies(Reparacio reparacio);

        /// <summary>
        /// Mètode per obtenir la llista de clients de la base de dades
        /// </summary>
        /// <returns>La llista de clients.</returns>
        List<Client> obtenirClients();

        /// <summary>
        /// Mètode per obtenir els vehicles d'un client
        /// </summary>
        /// <param name="client">Client propietari</param>
        /// <returns>Vehicles del client</returns>
        Client obtenirVehicles(Client client);

        /// <summary>
        /// Mètode per inserir una reparació a la base de dades
        /// </summary>
        /// <param name="reparacio">Reparacio a inserir</param>
        /// <returns>Confirmació</returns>
        bool insertarReparacio(Reparacio reparacio);

        /// <summary>
        /// Mètode per modificar una reparació a la base de dades
        /// </summary>
        /// <param name="reparacio">Reparació modificada</param>
        /// <returns>Confirmació</returns>
        bool modificarReparacio(Reparacio reparacio);

        /// <summary>
        /// Mètode per canviar l'estat d'una reparació
        /// </summary>
        /// <param name="reparacio">Reparació a modificar</param>
        /// <param name="estat">Nou estat de la reparació</param>
        /// <returns>Confirmació</returns>
        bool canviarEstatReparacio(Reparacio reparacio, EstatReparacio estat);

        /// <summary>
        /// Mètode per obtenir els packs de la base de dades
        /// </summary>
        /// <returns>Llista de packs</returns>
        List<Pack> obtenirPacks();

        /// <summary>
        /// Mètode per inserir una factura a la base de dades
        /// </summary>
        /// <param name="factura">Factura a inserir</param>
        /// <param name="emissor">Emissor de la factura</param>
        /// <returns>Confirmació</returns>
        bool insertarFactura(Factura factura, string emissor);

        /// <summary>
        /// Mètode per saber si la factura d'una reparació està pagada
        /// </summary>
        /// <param name="reparacio">Reparació a consultar</param>
        /// <returns>Si està pagada o no</returns>
        bool reparacioTeFacturaPagada(Reparacio reparacio);

        /// <summary>
        /// Mètode per pagar la factura d'una reparació
        /// </summary>
        /// <param name="reparacio">Reparació que volem pagar</param>
        /// <returns>Confirmació</returns>
        bool pagarFactura(Reparacio reparacio);


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
