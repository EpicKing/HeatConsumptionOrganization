using System;
using System.Collections.Generic;
using System.Text;

namespace HeatConsumptionOrganization
{
    class HeatConsumption
    {
        public int HeatConsumptionID { get; set; }

        public int TotalConsumed { get; set; }

        public int Year { get; set; }

        public int Quarter { get; set; }

        public int OrganizationID { get; set; }

        public virtual Organization Organization { get; set; }
    }
}
