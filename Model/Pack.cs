using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class Pack
    {
        private string nom;
        private decimal preu;

        public Pack(string nom, decimal preu)
        {
            Nom = nom;
            Preu = preu;
        }

        public string Nom { get => nom; set => nom = value; }
        public decimal Preu { get => preu; set => preu = value; }
    }
}
