using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace RFID_SYSTEM
{
    public partial class Posts
    {
        public Posts()
        {
            Employees = new HashSet<Employees>();
        }

        public int IdPost { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Employees> Employees { get; set; }
    }
}
