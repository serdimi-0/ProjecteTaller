using InterficiePersistencia;
using Microsoft.VisualBasic;
using Model;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace CapaMongoDB
{
    public class GestorBDTallerMongoDB : GestorBDTaller
    {
        MongoClient con;
        IMongoDatabase db;

        public GestorBDTallerMongoDB(NameValueCollection properties)
        {

            con = new MongoClient($"mongodb://{properties["ip"]}:{properties["port"]}/");
            comprovaConnexió();
            db = con.GetDatabase("taller");
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

        public Usuari? verificarUsuari(string username, string password)
        {
            var usuaris = db.GetCollection<BsonDocument>("usuaris");
            var usuariBson = usuaris.Find(new BsonDocument("login", username)).FirstOrDefault();
            if (usuariBson == null)
                return null;
            Usuari? usuari = new Usuari(usuariBson["login"].AsString, usuariBson["password"].AsString);
            string tipus = usuariBson["tipus"].AsString;
            switch (tipus)
            {
                case "admin":
                    usuari.Tipus = TipusUsuari.ADMIN;
                    break;
                case "mecanic":
                    usuari.Tipus = TipusUsuari.MECANIC;
                    break;
                case "recepcio":
                    usuari.Tipus = TipusUsuari.RECEPCIO;
                    break;
            }
            return usuari;
        }

        public List<Reparacio>? obtenirReparacions()
        {
            var reparacions = db.GetCollection<BsonDocument>("reparacions");
            var reparacionsBson = reparacions.Find(new BsonDocument()).ToList();
            List<Reparacio> reparacionsList = new List<Reparacio>();
            foreach (var reparacioBson in reparacionsBson)
            {
                string id = reparacioBson["vehicle_id"].AsString;
                EstatReparacio estat;
                switch (reparacioBson["estat"].AsString)
                {
                    case "oberta":
                        estat = EstatReparacio.OBERTA;
                        break;
                    case "tancada":
                        estat = EstatReparacio.TANCADA;
                        break;
                    case "facturada":
                        estat = EstatReparacio.FACTURADA;
                        break;
                    default:
                        throw new GestorBDTallerException();
                }
                DateTime data = reparacioBson["data"].ToUniversalTime();
                int numeroLinies = reparacioBson["linies"].AsBsonArray.Count;

                Reparacio reparacio = new Reparacio(id, estat, data, numeroLinies);
                reparacionsList.Add(reparacio);
            }
            return reparacionsList;
        }

        public Vehicle obtenirVehicle(string matricula)
        {
            var clients = db.GetCollection<BsonDocument>("clients");

            var filter = Builders<BsonDocument>.Filter.ElemMatch<BsonDocument>("vehicles", Builders<BsonDocument>.Filter.Eq("matricula", matricula));

            // Proyección para obtener solo el primer elemento del array de vehículos que coincida
            var projection = Builders<BsonDocument>.Projection.Include("vehicles.$").Exclude("_id");

            // Realizar la consulta
            var result = clients.Find(filter).Project(projection).FirstOrDefault();

            var vehicle = result["vehicles"][0].AsBsonDocument;


            return new Vehicle(matricula, vehicle["model"].AsString, vehicle["km"].AsInt32);
        }

        private void comprovaConnexió()
        {
            CancellationTokenSource cts = new CancellationTokenSource();
            CancellationToken token = cts.Token;

            Task task = Task.Run(() =>
            {
                con.ListDatabases().ToList();
            }, token);

            if (!task.Wait(5000, token))
            {
                cts.Cancel();
                throw new GestorBDTallerException();
            }
        }
    }
}