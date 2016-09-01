using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MKBLE;

namespace RedTooth.App
{
    public class Program
    {
        static void Main(string[] args)
        {
            ConnectionManager cm = new ConnectionManager();
            cm.OpenConnection();
            cm.Scan();
            Console.Read();
            cm.ResetAll();
        }
    }
}
