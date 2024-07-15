using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Model.All
{
    public class LocomativeInformation
    {
        public int loco_id { get; set; }
        public string name { get; set; }
        public FuelType fuel_type { get; set; }
    }
}
