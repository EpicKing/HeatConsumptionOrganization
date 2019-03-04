using System;
using System.Collections.Generic;
using System.Text;

namespace HeatConsumptionOrganization
{
    class Organization
    {
        public int OrganizationID { get; set; }

        public string Name { get; set; }

        public string TypeOfOwnership { get; set; }

        public string Address { get; set; }

        public string DirectorFullName { get; set; }

        public string DirectorPhoneNumber { get; set; }

        public string ChiefPowerEngineerFullName { get; set; }

        public string ChiefPowerEngineerPhoneNumber { get; set; }

        public virtual ICollection<TypeOfProduct> TypeOfProducts { get; set; }
        public virtual ICollection<ManufacturedProduct> ManufacturedProducts { get; set; }
        public virtual ICollection<HeatConsumption> HeatConsumptions { get; set; }
        public virtual ICollection<HeatConsumptionRate> HeatConsumptionRates { get; set; }
    }
}
