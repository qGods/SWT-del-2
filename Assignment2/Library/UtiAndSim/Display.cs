using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Library.Interface;

namespace Library.UtiAndSim
{
    public class Display : IDisplay
    {
        public void connectPhone()
        {
            Console.WriteLine("Please connect your phone");
        }

        public void scanRfid()
        {
            Console.WriteLine("Please scan your Rfid");
        }

        public void connectionError()
        {
            Console.WriteLine("Try reconnecting your phone");
        }

        public void occupied()
        {
            Console.WriteLine("The box is occupied");
        }

        public void rfidError()
        {
            Console.WriteLine("Try scanning again");
        }

        public void removePhone()
        {
            Console.WriteLine("Please remove your phone");
        }
    }
}
