using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace RFID_SYSTEM
{
    public partial class Entrances
    {
        public int IdEntr { get; set; }
        public int Employee { get; set; }
        public DateTime DateTime { get; set; }
        public string Direction { get; set; }

        public virtual Employees EmployeeNavigation { get; set; }
    }
}
