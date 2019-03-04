using System;
using System.Collections.Generic;
using System.Text;

namespace HeatConsumptionOrganization
{
    class TypeOfProduct
    {
        public int TypeOfProductID { get; set; }

        public string Name { get; set; }

        public string Unit { get; set; }

        public int OrganizationID { get; set; }

        public virtual Organization Organization { get; set; }
    }
}
