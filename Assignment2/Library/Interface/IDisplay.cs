using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Interface
{
    public interface IDisplay
    {
        public void connectPhone();

        public void scanRfid();

        public void connectionError();

        public void occupied();

        public void rfidError();

        public void removePhone();
        
    }
}
