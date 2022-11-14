using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Library.Control;
using static Library.Control.ChargingControl;

namespace Library.Interface
{
    public interface IChargingControl
    {
        public double currentValue { get;}
        public ChargeState state { get; set; }
        bool IsConnected { get; set; }
        void StartCharge();
        public void CurrentState();
        void StopCharge();

    }

}
