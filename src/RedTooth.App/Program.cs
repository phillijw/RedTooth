using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MKBLE;
using RedTooth.Core.Controller;
using RedTooth.Core.Model;
namespace RedTooth.App
{
    public class Program
    {
        static void Main(string[] args)
        {
            SortedDictionary<int, Tool> allTools;

            using (var tc = new ToolController())
            {
                System.Threading.Thread.Sleep(1000);

                allTools = tc.AllTools();
                foreach (var tool in allTools)
                {
                    if (tool.Value.MPBID == "0007000D36")
                        tc.SendCommand(tool.Value.ID);
                }
                Console.WriteLine("Press the any key to exit");
                Console.Read();
            }
            
        }
    }
}
