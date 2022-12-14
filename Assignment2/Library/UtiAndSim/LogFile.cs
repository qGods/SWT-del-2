using Library.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Library.UtiAndSim
{
    public class LogFile : ILogFile
    {

        public void logDoorLocked(string id )
        {
            using (StreamWriter sw = new StreamWriter("LogFile.txt"))
            {
                sw.WriteLine(DateTime.Now);
                sw.WriteLine("ID:",id);
                sw.Write("has locked the door");
            }
        }


        public void logDoorUnlocked(string id)
        {
            using (StreamWriter sw = new StreamWriter("LogFile.txt"))
            {
                sw.WriteLine(DateTime.Now);
                sw.WriteLine("ID:", id);
                sw.Write("has unlocked the door");
            }
        }
    }
}
