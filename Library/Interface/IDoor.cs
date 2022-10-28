using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Library.UtiAndSim;
using static Library.UtiAndSim.Door;

namespace Library.Interface
{
    public class DoorStateEventArgs : EventArgs
    {
        public DoorState DoorStateEvent { get; set; }

    }

    public interface IDoor
    {
        public void DoorUnlock();
        public void DoorLock();
        public void SetDoorState (DoorState newDoorState);

        event EventHandler<DoorStateEventArgs> DoorStateEvent;
        bool _open { get; set; }
        bool locked { get; set; }
        void DoorOpened();

        void DoorClosed();
    }

    public enum DoorState

    {
        //comments fra frank
        open,
        closed,
        DoorUnlock,
        DoorLock
    }

}
