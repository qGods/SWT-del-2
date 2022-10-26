using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Library.Control;

namespace Library.Interface
{
    public interface IChargingControl
    {
        public double currentValue { get;}
        bool IsConnected { get; set; }
        void StartCharge();
        void StopCharge();

    }

}
