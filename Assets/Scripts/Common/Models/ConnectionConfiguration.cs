using System;
using System.Collections.Generic;

namespace Assets.Scripts.Common.Models
{
    [Serializable]
    public class ConnectionConfiguration
    {
        public string IpAddress { get; set; }

        public string CpuType { get; set; }

        public short Rack { get; set; }

        public short Slot { get; set; }

        public List<string> VarAdressesOutput { get; set; }
        public List<string> VarAdressesInput { get; internal set; }
    }
}
