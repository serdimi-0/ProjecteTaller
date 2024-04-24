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
        PACK,
        ALTRES
    }
    public class Linia
    {
        private string descripcio;
        private decimal? preu;
        private decimal? preuUnitari;
        private TipusLinia tipus;
        private int? quantitat;
        private string? codiFabricant;
        private int? facturaId;

        public Linia()
        {
        }

        public Linia(string descripcio, decimal? preu, TipusLinia tipus, int? quantitat, string? codiFabricant, decimal? preuUnitari)
        {
            Descripcio = descripcio;
            Preu = preu;
            Tipus = tipus;
            Quantitat = quantitat;
            CodiFabricant = codiFabricant;
            PreuUnitari = preuUnitari;
        }

        public string Descripcio { get => descripcio; set => descripcio = value; }
        public decimal? Preu { get => preu; set => preu = value; }
        public String PreuString
        {
            get
            {
                return preu.HasValue ? preu.Value.ToString("0.00€") : "";
            }
        }
        public TipusLinia Tipus { get => tipus; set => tipus = value; }
        public string TipusString
        {
            get
            {
                switch (tipus)
                {
                    case TipusLinia.FEINA:
                        return "Feina";
                    case TipusLinia.PEÇA:
                        return "Peça";
                    case TipusLinia.PACK:
                        return "Pack";
                    case TipusLinia.ALTRES:
                        return "Altres";
                    default:
                        return "Desconegut";
                }
            }
        }
        public int? Quantitat { get => quantitat; set => quantitat = value; }
        public int? FacturaId { get => facturaId; set => facturaId = value; }
        public string? CodiFabricant { get => codiFabricant; set => codiFabricant = value; }
        public decimal? PreuUnitari { get => preuUnitari; set => preuUnitari = value; }
        public int Numero { get; set; }

    }
}
