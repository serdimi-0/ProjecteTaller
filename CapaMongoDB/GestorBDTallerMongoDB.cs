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
        public bool connectat = false;

        public GestorBDTallerMongoDB(NameValueCollection properties)
        {

            con = new MongoClient($"mongodb://{properties["ip"]}:{properties["port"]}/");
            try
            {
                comprovaConnexió();
                db = con.GetDatabase("taller");
                connectat = true;
            }
            catch (GestorBDTallerException e)
            {
                connectat = false;
            }
        }

        public bool connexioEstablerta()
        {
            return connectat;
        }

        public void confirmarCanvis()
        {
            // No funcionem amb transaccions
        }

        public void desferCanvis()
        {
            // No funcionem amb transaccions
        }

        public void tancarCapa()
        {
            con.Cluster.Dispose();
        }

        public Usuari? verificarUsuari(string username, string password)
        {
            var usuaris = db.GetCollection<BsonDocument>("usuaris");
            var usuariBson = usuaris.Find(new BsonDocument("login", username)).FirstOrDefault();
            if (usuariBson == null)
                return null;
            Usuari? usuari = new Usuari(usuariBson["login"].AsString, usuariBson["password"].AsString);
            if (usuari.Password != password)
                return null;
            string tipus = usuariBson["tipus"].AsString;
            switch (tipus)
            {
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
                    case "rebutjada":
                        estat = EstatReparacio.REBUTJADA;
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
                if (reparacio.Estat != EstatReparacio.REBUTJADA)
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
            int i = 1;
            foreach (var liniaBson in liniesBson)
            {
                // Omplim els camps que existeixen necessariament
                string descripcio = liniaBson["descripció"].AsString;
                Linia l;
                switch (liniaBson["tipus"].AsString)
                {
                    case "feina":
                        l = new Linia(descripcio, TipusLinia.FEINA);
                        break;
                    case "peça":
                        l = new Linia(descripcio, TipusLinia.PEÇA);
                        l.CodiFabricant = liniaBson["codi_fabricant"].AsString;
                        l.PreuUnitari = liniaBson["preu_unitat"].ToDecimal();
                        l.Quantitat = liniaBson["quantitat"].AsInt32;
                        break;
                    case "pack":
                        l = new Linia(descripcio, TipusLinia.PACK);
                        l.Preu = liniaBson["preu"].ToDecimal();
                        break;
                    case "altres":
                        l = new Linia(descripcio, TipusLinia.ALTRES);
                        l.Preu = liniaBson["preu"].ToDecimal();
                        break;
                    default:
                        throw new GestorBDTallerException();
                }
                l.Numero = i;

                // Omplim els camps opcionals
                try
                {
                    l.Quantitat = liniaBson["quantitat"].AsInt32;
                }
                catch (Exception e) { }

                try
                {
                    l.Preu = liniaBson["preu"].ToDecimal();
                }
                catch (Exception e) { }

                try
                {
                    l.Import = liniaBson["import"].ToDecimal();
                }
                catch (Exception e) { }

                try
                {
                    l.Descompte = liniaBson["descompte"].AsInt32;
                }
                catch (Exception e) { }



                linies.Add(l);
                i++;

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

        public bool insertarReparacio(Reparacio reparacio)
        {
            // insert reparacio and linies
            var reparacions = db.GetCollection<BsonDocument>("reparacions");

            BsonDocument reparacioBson = new BsonDocument
            {
                { "vehicle_id", reparacio.VehicleId },
                { "estat", reparacio.Estat.ToString().ToLower() },
                { "data", reparacio.Data },
                { "linies", new BsonArray() }
            };


            // En una nova reparació les línies han de ser feines totes
            var liniesBson = reparacioBson["linies"].AsBsonArray;
            foreach (Linia linia in reparacio.Linies)
            {
                BsonDocument liniaBson = new BsonDocument
                {
                    { "descripció", linia.Descripcio },
                    { "tipus", "feina" }
                };
                liniesBson.Add(liniaBson);
            }

            reparacions.InsertOne(reparacioBson);

            return true;
        }

        public bool modificarReparacio(Reparacio reparacio)
        {

            // replace whole reparacio and linies with new ones
            var reparacions = db.GetCollection<BsonDocument>("reparacions");

            BsonDocument reparacioBson = new BsonDocument
            {
                { "vehicle_id", reparacio.VehicleId },
                { "estat", reparacio.Estat.ToString().ToLower() },
                { "data", reparacio.Data },
                { "linies", new BsonArray() }
            };

            var liniesBson = reparacioBson["linies"].AsBsonArray;
            foreach (Linia linia in reparacio.Linies)
            {
                BsonDocument liniaBson = new BsonDocument
                {
                    { "descripció", linia.Descripcio },
                    { "tipus", linia.Tipus.ToString().ToLower() }
                };
                if (linia.Quantitat != null && linia.Quantitat != 0)
                    liniaBson.Add("quantitat", linia.Quantitat);

                if (linia.CodiFabricant != null && linia.CodiFabricant != "")
                    liniaBson.Add("codi_fabricant", linia.CodiFabricant);

                if (linia.PreuUnitari != null && linia.PreuUnitari != 0)
                    liniaBson.Add("preu_unitat", linia.PreuUnitari);

                if (linia.Preu != null && linia.Preu != 0)
                    liniaBson.Add("preu", linia.Preu);

                if (linia.Descompte != null && linia.Descompte != 0)
                    liniaBson.Add("descompte", linia.Descompte);

                if (linia.Import != null && linia.Import != 0)
                    liniaBson.Add("import", linia.Import);


                liniesBson.Add(liniaBson);
            }

            if (reparacio.Factura != null)
                reparacioBson.Add("factura", reparacio.Factura);

            reparacions.ReplaceOne(new BsonDocument("_id", ObjectId.Parse(reparacio.Id)), reparacioBson);


            return true;
        }

        public bool canviarEstatReparacio(Reparacio reparacio, EstatReparacio estat)
        {
            var reparacions = db.GetCollection<BsonDocument>("reparacions");

            // Get the current document
            var reparacioBson = reparacions.Find(new BsonDocument("_id", ObjectId.Parse(reparacio.Id))).FirstOrDefault();
            if (reparacioBson == null)
                return false;

            // Update the document
            var update = Builders<BsonDocument>.Update.Set("estat", estat.ToString().ToLower());
            reparacions.UpdateOne(new BsonDocument("_id", ObjectId.Parse(reparacio.Id)), update);


            return true;
        }

        public List<Pack> obtenirPacks()
        {
            List<Pack> list = new List<Pack>();
            var packs = db.GetCollection<BsonDocument>("packs");

            var packsBson = packs.Find(new BsonDocument()).ToList();
            foreach (var packBson in packsBson)
            {
                string nom = packBson["descripcio"].AsString;
                decimal preu = packBson["preu"].ToDecimal();
                list.Add(new Pack(nom, preu));
            }

            return list;
        }

        public bool insertarFactura(Factura factura, String emissor)
        {
            var factures = db.GetCollection<BsonDocument>("factures");
            var comptadors = db.GetCollection<BsonDocument>("comptadors");

            var comptadorBson = comptadors.Find(new BsonDocument("nom", "factures")).FirstOrDefault();
            if (comptadorBson == null)
                throw new GestorBDTallerException();

            int facturaNum = comptadorBson["valor_ultim"].AsInt32 + 1;


            BsonDocument facturaBson = new BsonDocument
            {
                { "numero", facturaNum },
                { "estat", "pendent" },
                { "data", factura.Data },
                { "tipus_IVA", factura.TipusIva },
                { "preu_ma_obra", factura.PreuMaObra },
                { "reparacio_id", ObjectId.Parse(factura.ReparacioId) },
                { "subtotal", factura.Subtotal },
                { "import_IVA", factura.Iva },
                { "total", factura.Total },
                { "emissor", emissor }
            };

            factures.InsertOne(facturaBson);
            var update = Builders<BsonDocument>.Update.Set("valor_ultim", facturaNum);
            comptadors.UpdateOne(new BsonDocument("nom", "factures"), update);

            return true;
        }

        public bool reparacioTeFacturaPagada(Reparacio reparacio)
        {
            var factures = db.GetCollection<BsonDocument>("factures");

            var facturaBson = factures.Find(new BsonDocument("reparacio_id", ObjectId.Parse(reparacio.Id))).FirstOrDefault();
            if (facturaBson == null)
                return false;

            if (facturaBson["estat"].AsString == "pagada")
                return true;
            else
                return false;
        }

        public bool pagarFactura(Reparacio reparacio)
        {
            var factures = db.GetCollection<BsonDocument>("factures");

            var facturaBson = factures.Find(new BsonDocument("reparacio_id", ObjectId.Parse(reparacio.Id))).FirstOrDefault();
            if (facturaBson == null)
                return false;

            var update = Builders<BsonDocument>.Update.Set("estat", "pagada");
            factures.UpdateOne(new BsonDocument("reparacio_id", ObjectId.Parse(reparacio.Id)), update);

            return true;
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