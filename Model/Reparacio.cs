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
        FACTURADA
    }

    public class Reparacio
    {
        private string vehicleId;
        private EstatReparacio estat;
        private DateTime data;
        private int numeroLinies;

        public Reparacio(string vehicleId, EstatReparacio estat, DateTime data, int numeroLinies)
        {
            this.vehicleId = vehicleId;
            this.estat = estat;
            this.data = data;
            this.numeroLinies = numeroLinies;
        }

        public string VehicleId { get => vehicleId; set => vehicleId = value; }
        public EstatReparacio Estat { get => estat; set => estat = value; }
        public DateTime Data { get => data; set => data = value; }
        public int NumeroLinies { get => numeroLinies; set => numeroLinies = value; }
        public string Model { get; set; }
        public string DataString { get => data.ToString("dd/MM/yyyy"); }

        public override string ToString()
        {
            return $"REPARACIÓ {vehicleId}: estat {estat}, data {data}, numero de línies {numeroLinies}";
        }
    }
}
