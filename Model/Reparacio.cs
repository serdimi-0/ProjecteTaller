using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public enum EstatReparacio
    {
        OBERTA,
        TANCADA,
        FACTURADA,
        REBUTJADA
    }

    public class Reparacio
    {
        private string id;
        private string vehicleId;
        private EstatReparacio estat;
        private DateTime data;
        private int numeroLinies;
        private List<Linia> linies;
        private int? factura = null;
        private bool facturaPagada = false;

        public Reparacio(string vehicleId, EstatReparacio estat, DateTime data, int numeroLinies)
        {
            this.vehicleId = vehicleId;
            this.estat = estat;
            this.data = data;
            this.numeroLinies = numeroLinies;
        }

        public string Id { get => id; set => id = value; }
        public string VehicleId { get => vehicleId; set => vehicleId = value; }
        public EstatReparacio Estat { get => estat; set => estat = value; }
        public string EstatString
        {
            get
            {
                switch (estat)
                {
                    case EstatReparacio.OBERTA:
                        return "Oberta";
                    case EstatReparacio.TANCADA:
                        return "Tancada";
                    case EstatReparacio.FACTURADA:
                        return "Facturada";
                    default:
                        return "Desconegut";
                }
            }
        }
        public DateTime Data { get => data; set => data = value; }
        public int NumeroLinies { get => numeroLinies; set => numeroLinies = value; }
        public string Model { get; set; }
        public string DataString { get => data.ToString("dd/MM/yyyy"); }
        public List<Linia> Linies { get => linies; set => linies = value; }
        public int? Factura { get => factura; set => factura = value; }
        public bool FacturaPagada { get => facturaPagada; set => facturaPagada = value; }

        public override string ToString()
        {
            return $"REPARACIÓ {vehicleId}: estat {estat}, data {data}, numero de línies {numeroLinies}";
        }
    }
}
