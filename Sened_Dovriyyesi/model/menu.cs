using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sened_Dovriyyesi.model
{
    public class menu
    {
        public int id { get; set; }
        public int parentID { get; set; }
        public int docTypeID { get; set; }
        public string iconClass { get; set; }
        public string caption { get; set; }
    }
}
