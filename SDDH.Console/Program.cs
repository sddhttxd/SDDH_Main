using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDDH.Consoles
{
    public class Program
    {
        public static void Main(string[] args)
        {
            DoManager();
        }

        private static void DoManager() {

            Console.WriteLine("Start!");
            JobManager.RunDemo().Wait();
            Console.Read();
        }
    }
}
