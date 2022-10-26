using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.UtiAndSim
{
    public class LogFile
    {

        public void logDoorLocked(string id )
        {
            using (StreamWriter sw = new StreamWriter("LogFile.txt"))
            {
                sw.WriteLine(DateTime.Now);
                sw.WriteLine(id);
            }
        }


        public void logDoorUnlocked(string id)
        {
            
        }
    }
}
