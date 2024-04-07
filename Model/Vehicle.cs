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
        private string marcaModel;
        private int km;

        public Vehicle(string matricula, string marcaModel, int km)
        {
            this.matricula = matricula;
            this.marcaModel = marcaModel;
            this.km = km;
        }

        public string Matricula { get => matricula; set => matricula = value; }
        public string MarcaModel { get => marcaModel; set => marcaModel = value; }
        public int Km { get => km; set => km = value; }
    }
}
