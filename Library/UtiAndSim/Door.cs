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

        public bool locked { get; set; }   
        public bool _open { get; set; }     
        public Door()
        {
            _doorState = DoorState.open;
            //SetDoorState(DoorState.open);
        }

        //This method calls "OnDorrState", which will raise an event
        public void SetDoorState(DoorState newDoorState)
        {
            if (newDoorState != _doorState)
            {
                Console.WriteLine("Door is now " + newDoorState);
                OnDoorState();
                _doorState = newDoorState;
            }
            else
            {
                Console.WriteLine("Door is already "+_doorState);
            }

        }
        //TODO: Instead of calling SetDoorState from other classes, make them call doorlock / doorunlock which then calls setdoorstate
        
        protected virtual void OnDoorState()
        {
            DoorStateEvent?.Invoke(this, new DoorStateEventArgs());
        }

        public void DoorLock()
        {
            SetDoorState(DoorState.closed);
        }

        public void DoorUnlock()
        {
            SetDoorState(DoorState.open);

        }

        public void DoorOpened()
        {
            if (!locked)
            {
                _open = true;
                OnDoorState();            
            }


        }

        public void DoorClosed()
        {
            _open = false;
            OnDoorState();    
        }
    }
}
