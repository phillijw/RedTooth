using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedTooth.Core.Model
{
    public enum  BluetoothState :int
    {
        STATE_STANDBY = 0,
        STATE_SCANNING = 1,
        STATE_CONNECTING = 2,
        STATE_FINDING_SERVICES = 3,
        STATE_FINDING_ATTRIBUTES = 4,
        STATE_LISTENING_MEASUREMENTS = 5
    }

    public class Tool
    {
        public String MPBID { get; set; }
        public Byte[] BluetoothAddress { get; set; }
        public int ID { get; set; }
        public byte AddressType { get; set; }
        public int RSSI { get; set; }
        public BluetoothState BLEState { get; set; }
        public byte ConnectionHandle { get; set; }
    }
}
