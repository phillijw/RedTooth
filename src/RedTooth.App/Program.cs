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
                allTools = tc.AllTools();
                Console.WriteLine("Press the any key to exit");
                Console.Read();
            }
            
        }
    }
}
