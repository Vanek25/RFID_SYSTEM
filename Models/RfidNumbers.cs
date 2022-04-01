using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace RFID_SYSTEM
{
    public partial class RfidNumbers
    {
        public RfidNumbers()
        {
            Employees = new HashSet<Employees>();
        }

        public int IdRfid { get; set; }
        public string Number { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime? ValidTo { get; set; }

        public virtual ICollection<Employees> Employees { get; set; }
    }
}
