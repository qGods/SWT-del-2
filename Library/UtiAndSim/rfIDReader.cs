using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Library.Control;
using Library.Interface;
using Library.UtiAndSim;

namespace Library.UtiAndSim
{
    
    public class rfIDReader :  IrfIDReader
    {
        public event EventHandler<rfIDDetectedArgs> rfIDEvent;

        public void ReadrfID(int ID)
        {
            if (ID <=0) {return;}

            OnReadrfID(new rfIDDetectedArgs { rfIDDetected = ID });
        }

        protected virtual void OnReadrfID(rfIDDetectedArgs e)
        {
            rfIDEvent?.Invoke(this, e);
        }
    }
}
