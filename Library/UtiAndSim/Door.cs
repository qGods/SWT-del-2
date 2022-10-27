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
            SetDoorState(DoorState.open);
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
        //TODO: Instead of calling SetDoorState from other classes, make them call doorlock / doorunlock which then calls setdoorstate
        
        protected virtual void OnDoorState(DoorStateEventArgs e)
        {
            //call all the event handler methods which is registered with "DoorStateEvent"
            //"subsribers" are the classes which will register to "DorStateEvent"
            DoorStateEvent?.Invoke(this, e);
        }

        public void DoorLock()
        {
            Console.WriteLine("Door locked");
            _doorState = DoorState.closed;
        }

        public void DoorUnlock()
        {
            _doorState = DoorState.open;
        }
    }
}
