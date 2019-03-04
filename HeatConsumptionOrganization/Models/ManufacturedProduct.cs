using System;
using System.Collections.Generic;
using System.Text;

namespace HeatConsumptionOrganization
{
    class ManufacturedProduct
    {
        public int ManufacturedProductID { get; set; }

        public int TotalProduced { get; set; }

        public int Year { get; set; }

        public int Quarter { get; set; }

        public int OrganizationID { get; set; }

        public virtual Organization Organization { get; set; }
    }
}
