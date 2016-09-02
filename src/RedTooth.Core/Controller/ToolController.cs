using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using MKBLE;
using RedTooth.Core.Model;
using RedTooth.Core;

namespace RedTooth.Core.Controller
{
    public class ToolController : IDisposable
    {
        private ConnectionManager cm;
        private SortedDictionary<int, Tool> _allTools = new SortedDictionary<int, Tool>();
        public ToolController()
        {
            //Conn
            cm = new ConnectionManager();
            System.Threading.Thread.Sleep(1000);
        }

        public SortedDictionary<int, String> AllMpbid(int SortSelector = 0)
        {
            return new SortedDictionary<int, string>();
        }

        public SortedDictionary<int, Tool> AllTools(int SortSelector = 0)
        {
            Dictionary<String, Tool> allTools = cm.localTools;
            _allTools = new SortedDictionary<int, Tool>();

            foreach (var tool in allTools)
            {
                _allTools.Add(tool.Value.ID, tool.Value);
            }

            return _allTools;
        }

        public bool Connect(int toolID)
        {
            return true;
        }

        //for now just send the tool ID and we will hardcode the command. We could provide a menu later.
        public bool SendCommand(int toolID)
        {
            Tool toolToConnect = _allTools[toolID];
            cm.ConnectToTool(toolToConnect);
            return true;
        }

        public void Dispose()
        {
            cm.Dispose();
        }

        public static SortedDictionary<int, Tool> MockTools(int SortSelector = 0)
        {
            var tools = new SortedDictionary<int, Tool>();
            var numGen = new Random();

            byte[] bytes = new byte[256];
            numGen.NextBytes(bytes);

            for (int i = 0; i < 10; ++i)
            {
                var tool = new Tool
                {
                    AddressType = bytes[i],
                    BLEState = BluetoothState.STATE_SCANNING,
                    BluetoothAddress = bytes.Take(4).ToArray(),
                    ConnectionHandle = bytes[i + 1],
                    ID = i,
                    MPBID = ByteArrayToString(bytes.Take(4).ToArray()),
                    RSSI = numGen.Next()
                };
                tool.Name = LookupProductName(tool.MPBID);
                tools.Add(i, tool);
            }

            return tools;
        }

        private static string LookupProductName(string MPBID)
        {
            string ProductName = "";

            int ProductId = Convert.ToInt16(ByteArrayToString(Encoding.UTF8.GetBytes(MPBID).Take(2).ToArray()));

            Dictionary<int, string> products = new Dictionary<int, string>
            {
                {1,"Compact Brushless Drill Driver"},
                {3,"Compact Brushless Impact Driver"},
                {8,"FUEL G2 Impact Driver - 1/4\" Hex"},
                {12,"FUEL G2 Impact Driver - 1/4\" Hex w/Bluetooth"},
                {2,"Compact Brushless Hammer Drill"},
                {4,"FUEL G2 Drill Driver"},
                {5,"FUEL G2 Hammer Drill"},
                {6,"FUEL G2 Drill Driver w/Bluetooth"},
                {7,"FUEL G2 Hammer Drill w/Bluetooth"},
                {9,"FUEL G2 Impact Wrench - 3/8\" Friction Ring"},
                {10,"FUEL G2 Impact Wrench - 1/2\" Friction Ring "},
                {11,"FUEL G2 Impact Wrench - 1/2\" Detent Pin"},
                {13,"FUEL G2 Impact Wrench - 3/8\" FR w/Bluetooth"},
                {14,"FUEL G2 Impact Wrench - 1/2\" FR w/Bluetooth"},
                {15,"FUEL G2 Impact Wrench - 1/2\" DP w/Bluetooth"},
                {16,"6T Hydraulics - Utility Crimper"},
                {20,"6T Hydraulics - Commercial Crimper U Die"},
                {21,"6T Hydraulics - Cutter"},
                {23,"OpenLink Adaptor"},
                {24,"FUEL Super Hawg 1/2\" Chuck"},
                {25,"FUEL Super Hawg Quik-Loc"},
                {26,"FUEL Braking Grinder"},
                {36,"M18 Battery Pack 1.5Ahr CP"},
                {37,"M18 Battery Pack 2.0Ahr CP"},
                {38,"M18 Battery Pack 3.0Ahr XC"},
                {39,"M18 Battery Pack 4.0Ahr XC"},
                {40,"M18 Battery Pack 5.0Ahr XC"},
                {27,"M18 FUEL™ 18ga Brad Nailer"},
                {41,"M18 FUEL™ 16ga Straight Finish Nailer Kit"},
                {42,"M18 FUEL™ 16ga Angled Finish Nailer Kit"},
                {43,"M18 FUEL™ 15ga Finish Nailer Kit"},
                {45,"FUEL Sawzall"},
                {51,"M18 12T Utility Crimper"},
                {52,"M18 12T Commercial Crimper"},
                {18,"Compact Site Light Single Pack"},
                {28,"M18 Compact Site Light DP One-Key"},
                {46,"M18 Battery Pack 6.0Ahr XC"},
                {47,"M18 Battery Pack 9.0Ahr HD"}
            };

            if (products.ContainsKey(ProductId))
            {
                ProductName = products[ProductId];
            }
            return ProductName;
        }

        public static string ByteArrayToString(byte[] ba)
        {
            string hex = BitConverter.ToString(ba);
            return hex.Replace("-", "");
        }
    }
}
