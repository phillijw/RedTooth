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
    public class ToolControler
    {
        private ConnectionManager cm;
        public ToolControler()
        {
            //Conn
            cm = new ConnectionManager();
        }

        public SortedDictionary<int, String> AllMpbid(int SortSelector = 0)
        {
            return new SortedDictionary<int, string>();
        }

        public SortedDictionary<int, Tool> AllTools(int SortSelector = 0)
        {
            Dictionary<String, Tool> allTools = cm.localTools;
            SortedDictionary<int, Tool> returnTools = new SortedDictionary<int, Tool>();
            foreach (var tool in allTools)
            {

                returnTools.Add(tool.Value.ID, tool.Value);
            }

            return returnTools;
            
        }
        

        public bool Connect(int toolID)
        {
            return true;
        }

        //for now just send the tool ID and we will hardcode the command. We could provide a menu later.
        public bool SendCommand(int toolID)
        {
            return true;
        }
    }
}
