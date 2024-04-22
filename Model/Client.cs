using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class Client
    {
        private string id;
        private string nif;
        private string nom;
        private string cognoms;
        private string telefon;
        private string direccio;
        private List<Vehicle> vehicles;

        public Client(string id, string nif, string nom, string cognoms, string telefon, string direccio)
        {
            this.id = id;
            this.nif = nif;
            this.nom = nom;
            this.cognoms = cognoms;
            this.telefon = telefon;
            this.direccio = direccio;
            vehicles = new List<Vehicle>();
        }

        public string Id { get => id; set => id = value; }
        public string Nif { get => nif; set => nif = value; }
        public string Nom { get => nom; set => nom = value; }
        public string Cognoms { get => cognoms; set => cognoms = value; }
        public string Telefon { get => telefon; set => telefon = value; }
        public string Direccio { get => direccio; set => direccio = value; }
        public List<Vehicle> Vehicles { get => vehicles; set => vehicles = value; }
    }
}
