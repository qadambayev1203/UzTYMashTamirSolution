using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Model.All
{
    public class Designations
    {
        public int designations_id { get; set; }
        public OrganizationType organization { get; set; }
        public string work_done { get; set; }
        public string unit_of_measure { get; set; }
    }
}
