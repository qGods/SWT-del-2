using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Interface
{
    public interface ILogFile
    {
        public void logDoorLocked(string ID);
        public void logDoorUnlocked(string ID);
    }
}
