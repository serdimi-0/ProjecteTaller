﻿using InterficiePersistencia;
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
                string id = reparacioBson["_id"].AsObjectId.ToString();
                string matricula = reparacioBson["vehicle_id"].AsString;
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
                int? factura = null;
                try
                {
                    factura = reparacioBson["factura"].AsInt32;
                }
                catch (Exception e)
                {
                }

                Reparacio reparacio = new Reparacio(matricula, estat, data, numeroLinies);
                reparacio.Factura = factura;
                reparacio.Id = id;
                reparacionsList.Add(reparacio);
            }
            return reparacionsList;
        }

        public Reparacio obtenirLinies(Reparacio reparacio)
        {

            var reparacions = db.GetCollection<BsonDocument>("reparacions");

            var reparacioBson = reparacions.Find(new BsonDocument("_id", ObjectId.Parse(reparacio.Id))).FirstOrDefault();
            if (reparacioBson == null)
                throw new GestorBDTallerException();

            var liniesBson = reparacioBson["linies"].AsBsonArray;
            List<Linia> linies = new List<Linia>();
            foreach (var liniaBson in liniesBson)
            {
                string numero = liniaBson["numero"].AsString;
                string descripcio = liniaBson["descripció"].AsString;
                decimal? preu;
                try
                {
                    preu = liniaBson["preu"].ToDecimal();
                }
                catch (Exception e)
                {
                    preu = null;
                }
                TipusLinia tipus;
                int? quantitat;
                string? codiFabricant;
                decimal? preuUnitari;
                switch (liniaBson["tipus"].AsString)
                {
                    case "feina":
                        tipus = TipusLinia.FEINA;
                        quantitat = liniaBson["quantitat"].AsInt32;
                        codiFabricant = null;
                        preuUnitari = null;
                        break;
                    case "peça":
                        tipus = TipusLinia.PEÇA;
                        quantitat = liniaBson["quantitat"].AsInt32;
                        codiFabricant = liniaBson["codi_fabricant"].AsString;
                        preuUnitari = liniaBson["preu_unitat"].ToDecimal();
                        break;
                    case "pack":
                        tipus = TipusLinia.PACK;
                        quantitat = null;
                        codiFabricant = null;
                        preuUnitari = null;
                        break;
                    default:
                        throw new GestorBDTallerException();
                }
                linies.Add(new Linia(numero, descripcio, preu, tipus, quantitat, codiFabricant, preuUnitari));

            }
            reparacio.Linies = linies;

            return reparacio;
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

        public List<Client> obtenirClients()
        {
            var clients = db.GetCollection<BsonDocument>("clients");
            var clientsBson = clients.Find(new BsonDocument()).ToList();
            List<Client> clientsList = new List<Client>();
            foreach (var clientBson in clientsBson)
            {
                string id = clientBson["_id"].AsObjectId.ToString();
                string nom = clientBson["nom"].AsString;
                string cognoms = clientBson["cognoms"].AsString;
                string dni = clientBson["nif"].AsString;
                string adreça = clientBson["adreça"].AsString;
                string telefon = clientBson["telefon"].AsString;
                List<Vehicle> vehicles = new List<Vehicle>();
                clientsList.Add(new Client(id, dni, nom, cognoms, telefon, adreça));
            }
            return clientsList;
        }

        public Client obtenirVehicles(Client client)
        {
            var clients = db.GetCollection<BsonDocument>("clients");

            var clientBson = clients.Find(new BsonDocument("_id", ObjectId.Parse(client.Id))).FirstOrDefault();
            if (clientBson == null)
                throw new GestorBDTallerException();

            var vehiclesBson = clientBson["vehicles"].AsBsonArray;
            List<Vehicle> vehicles = new List<Vehicle>();
            foreach (var vehicleBson in vehiclesBson)
            {
                string matricula = vehicleBson["matricula"].AsString;
                string model = vehicleBson["model"].AsString;
                int km = vehicleBson["km"].AsInt32;
                vehicles.Add(new Vehicle(matricula, model, km));
            }
            client.Vehicles = vehicles;

            return client;
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