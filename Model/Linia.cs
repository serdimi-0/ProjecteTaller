using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public enum TipusLinia
    {
        FEINA,
        PEÇA,
        PACK
    }
    public class Linia
    {
        private string numero;
        private string descripcio;
        private double preu;
        private TipusLinia tipus;
        private int quantitat;
        private int facturaId;

    }
}
