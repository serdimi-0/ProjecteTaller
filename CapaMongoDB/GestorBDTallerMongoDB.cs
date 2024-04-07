using InterficiePersistencia;
using Model;
using MongoDB.Driver;
using System.Collections.Specialized;

namespace CapaMongoDB
{
    public class GestorBDTallerMongoDB : GestorBDTaller
    {
        MongoClient con;

        public GestorBDTallerMongoDB(NameValueCollection properties)
        {

            con = new MongoClient($"mongodb://{properties["ip"]}:{properties["port"]}/");
            comprovaConnexió();
        }

        public void confirmarCanvis()
        {
            throw new NotImplementedException();
        }

        public void desferCanvis()
        {
            throw new NotImplementedException();
        }

        public void tancarCapa()
        {
            throw new NotImplementedException();
        }

        public Usuari verificarUsuari(string username, string password)
        {
            throw new NotImplementedException();
        }

        private void comprovaConnexió()
        {
            Thread t = new Thread(() =>
            {
                try
                {
                    con.ListDatabases().ToList();
                }
                catch (ThreadInterruptedException)
                {
                    throw new GestorBDTallerException("No s'ha pogut connectar a la base de dades");
                }
            });

            t.Start();
            if (!t.Join(5000))
            {
                t.Interrupt();
            }

        }
    }
}