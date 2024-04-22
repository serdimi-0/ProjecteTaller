using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class Vehicle
    {
        private string matricula;
        private string model;
        private int km;

        public Vehicle(string matricula, string model, int km)
        {
            this.matricula = matricula;
            this.model = model;
            this.km = km;
        }

        public string Matricula { get => matricula; set => matricula = value; }
        public string Model { get => model; set => model = value; }
        public int Km { get => km; set => km = value; }

        public string kmString
        {
            get
            {
                return km.ToString() + " km";
            }
        }
    }
}
