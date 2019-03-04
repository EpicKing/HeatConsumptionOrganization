using System;
using System.Collections.Generic;
using System.Text;

namespace HeatConsumptionOrganization
{
    class HeatConsumptionRate
    {
        public int HeatConsumptionRateID { get; set; }

        public int QuarterConsuptionRate { get; set; }

        public int Year { get; set; }

        public int Quarter { get; set; }

        public string ResponsibleOfficer { get; set; }

        public int OrganizationID { get; set; }

        public virtual Organization Organization { get; set; }
    }
}
