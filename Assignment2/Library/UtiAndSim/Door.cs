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
        public event EventHandler<DoorStateEventArgs> DoorStateEvent; //event

        private DoorState _doorState;


        public Door()
        {
            SetDoorState(DoorState.unlocked);
        }

        //This method calls "OnDorrState", which will raise an event
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
            //call all the event handler methods which is registered with "DoorStateEvent"
            //"subsribers" are the classes which will register to "DorStateEvent"
            DoorStateEvent?.Invoke(this, e);
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
