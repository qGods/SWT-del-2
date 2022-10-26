using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Library.Interface;

namespace Library.UtiAndSim
{
    public class Door : IDoor
    {
        public event EventHandler<DoorStateEventArgs> DoorStateEvent;

        private DoorState _doorState;




        public void SetDoorState(DoorState newDoorState)
        {
            if (newDoorState != _doorState)
            {
                OnDoorState(new DoorStateEventArgs { DoorStateEvent = newDoorState });
                _doorState = newDoorState;
            }

        }

        protected virtual void OnDoorState(DoorStateEventArgs e)
        {
            DoorStateEvent?.Invoke(this, e);
        }

        public Door()
        {
            SetDoorState(DoorState.unlocked);
        }

        public void DoorLock()
        {
            _doorState = DoorState.locked;
        }

        public void DoorUnlock()
        {
            _doorState = DoorState.unlocked;
        }
    }
}
