using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public enum EstatFactura
    {
        PENDENT,
        PAGADA
    }

    public class Factura
    {
        private string reparacioId;
        private int numero;
        private EstatFactura estat;
        private DateTime data;
        private int tipusIva;
        private decimal preuMaObra;
        private decimal subtotal;
        private decimal iva;
        private decimal total;

        public Factura(string reparacioId)
        {
            ReparacioId = reparacioId;
            Estat = EstatFactura.PENDENT;
        }

        public string ReparacioId { get => reparacioId; set => reparacioId = value; }
        public int Numero { get => numero; set => numero = value; }
        public EstatFactura Estat { get => estat; set => estat = value; }
        public DateTime Data { get => data; set => data = value; }
        public int TipusIva { get => tipusIva; set => tipusIva = value; }
        public decimal PreuMaObra { get => preuMaObra; set => preuMaObra = value; }
        public decimal Subtotal { get => subtotal; set => subtotal = value; }
        public decimal Iva { get => iva; set => iva = value; }
        public decimal Total { get => total; set => total = value; }
    }
}
