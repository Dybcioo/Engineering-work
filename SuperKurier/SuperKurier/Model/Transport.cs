using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperKurier.Model
{
    class Transport
    {
        public int ParcelId { get; set; }
        public string ParcelCode { get; set; }
        public string WarehouseFromCode { get; set; }
        public string WarehouseToCode { get; set; }
    }
}
