using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sened_Dovriyyesi.model
{
    public class docs
    {
        public int ID { get; set; }
        public string DirectionTypeName { get; set; }
        public string DocEnterNo { get; set; }
        public string DocEnterdate { get; set; }
        public int DocumentStatusID { get; set; }
        public string DocumentStatusName { get; set; }
        public int DocControlStatusID { get; set; }

    }
}
