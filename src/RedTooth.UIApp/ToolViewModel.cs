using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedTooth.UIApp
{
    public class ToolViewModel
    {
        public string Name { get; set; }
        public string BluetoothAddress { get; set; }        
        public string MPBID { get; set; }        
        public int RSSI { get; set; }        
    }
}
