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
        TANCADA
    }

    public class Reparacio
    {
        private string vehicleId;
        private EstatReparacio estat;
        private DateTime data;

        public Reparacio(string vehicleId, EstatReparacio estat, DateTime data)
        {
            this.vehicleId = vehicleId;
            this.estat = estat;
            this.data = data;
        }

        public string VehicleId { get => vehicleId; set => vehicleId = value; }
        public EstatReparacio Estat { get => estat; set => estat = value; }
        public DateTime Data { get => data; set => data = value; }
    }
}
