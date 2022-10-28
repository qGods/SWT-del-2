using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Interface
{
    public class rfIDDetectedArgs : EventArgs
    { 
        public int rfIDDetected { get; set; }
        public int ID { get; set; }
    }

    public interface IrfIDReader
    {
        void ReadrfID(int ID);
        event EventHandler<rfIDDetectedArgs> rfIDEvent;
    }
}
