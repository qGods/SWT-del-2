using Library.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Library.Interface;

namespace Library.UtiAndSim
{
    public class LogFile : ILogFile
    {

        public void logDoorLocked(int ID )
        {
            using (StreamWriter sw = new StreamWriter("LogFile.txt"))
            {
                sw.WriteLine(DateTime.Now);
                sw.WriteLine("ID:{0} has locked the door", ID);
            }
        }


        public void logDoorUnlocked(int ID)
        {
            using (StreamWriter sw = new StreamWriter("LogFile.txt"))
            {
                sw.WriteLine(DateTime.Now);
                sw.WriteLine("ID:{0} has unlocked the door ", ID);
            }
        }
    }
}
