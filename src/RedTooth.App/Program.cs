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

            ToolControler tc = new ToolControler();
            SortedDictionary<int, Tool> tools = tc.AllTools();


            //ConnectionManager cm = new ConnectionManager();

            //cm.OpenConnection();
            //cm.Scan();

            ConnectionManager cm = new ConnectionManager();
            //cm.OpenConnection();
            //cm.Scan();

            Console.Read();
            cm.ResetAll();
        }
    }
}
