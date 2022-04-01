using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace RFID_SYSTEM
{
    public partial class Employees
    {
        public Employees()
        {
            Entrances = new HashSet<Entrances>();
        }

        public int IdEmp { get; set; }
        public string Fio { get; set; }
        public int Company { get; set; }
        public byte[] Photo { get; set; }
        public int Rfid { get; set; }
        public int Post { get; set; }

        public virtual Companies CompanyNavigation { get; set; }
        public virtual Posts PostNavigation { get; set; }
        public virtual RfidNumbers Rf { get; set; }
        public virtual ICollection<Entrances> Entrances { get; set; }
    }
}
