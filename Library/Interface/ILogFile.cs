using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Interface
{
    public interface ILogFile
    {
        public void logDoorLocked(string id);
        public void logDoorUnlocked(string id);
    }
}
