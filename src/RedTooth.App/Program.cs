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
<<<<<<< HEAD
            
            ToolControler tc = new ToolControler();
            SortedDictionary<int, Tool> tools = tc.AllTools();


            //ConnectionManager cm = new ConnectionManager();

            //cm.OpenConnection();
            //cm.Scan();
=======
            ConnectionManager cm = new ConnectionManager();
            cm.OpenConnection();
            cm.Scan();
>>>>>>> 59ea31ccbaee2e79de7ba906354ed9262aa69621
            Console.Read();
            //cm.ResetAll();
        }
    }
}
