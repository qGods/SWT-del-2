using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Library.Interface;
using Library.UtiAndSim;

namespace Library.Control
{
    public class StationControl
    {
        // Enum med tilstande ("states") svarende til tilstandsdiagrammet for klassen
        private enum LadeskabState
        {
            Available,
            Locked,
            DoorOpen
        };

        public  LadeskabState _state;
        private IChargingControl _charger;
        public int _oldId { get; private set; }
        private IDoor _door;
        private IDisplay _display;
        private IrfIDReader _rfIDReader;
        private ILogFile _LogFile;

        public DoorState _doorEvent { get; private set; }
        public int _rfIDEvent { get; private set;}

        private string logFile = "logfile.txt";


        public StationControl(IChargingControl charger, IDoor door, IDisplay display, IrfIDReader rfIDReader, ILogFile LogFile)
        {
            door.DoorStateEvent += HandleDoorStateEvent;

            rfIDReader.rfIDEvent += RfidDetected;

            _door = door;

            _charger = charger;

            _display = display;

            _LogFile = LogFile;

            _rfIDReader = rfIDReader;

            _state = LadeskabState.Available;
        }



        private void RfidDetected(object? rfIDReader, rfIDDetectedArgs e)
        {
            if (_state == LadeskabState.Available)
            {
                if (!_charger.IsConnected())
                {
                    _display.NotConnected();
                    using (var writer = File.AppendText(logFile))
                    {
                        writer.WriteLine(DateTime.Now + "Phone Not Connected", e.rfIDDetected);
                    }
                }
                _charger.StartCharge();

                _door.DoorLock();
                _oldId = ID;

                _LogFile.logDoorLocked(_oldId);

                _state = LadeskabState.Locked;

                else if (_state == LadeskabState.Locked)
                {
                    if (CheckID(ID))
                    {
                        _charger.StopCharge();
                        _door.DoorUnlock();
                        _LogFile.logDoorUnlocked(ID);
                        _state = LadeskabState.Available;
                        _display.removePhone();
                    }
                    else
                    {
                        _display.occupied();
                    }
                }
            }
        }

        private bool CheckID(int ID)
        {
            if (ID == _oldId)
            {
                return true;
            }

            else
            {
                _display.rfidError();
                return false;
            }
        }

        private void DoorOpen()
        {
            _display.connectPhone();
            _charger.IsConnected = true;
        }

        private void DoorClose()
        {
            _display.scanRfid();
            _charger.IsConnected = false;
        }

        private void LockedDoor()
        {
            _display.occupied();
        }

        private bool IsConnected()
        {
            return _charger.IsConnected;
        }

        private void HandleDoorStateEvent(object sender, DoorStateEventArgs e)
        {
            switch (e.DoorStateEvent)
            {
                case DoorState.open:

                    _display.connectPhone();
                    break;

                case DoorState.closed:
                    _charger.StartCharge();
                    _display.scanRfid();
                    break;
            }
        }

        private void AvailableState(int ID)
        {
            if (!IsConnected())
            {
                _display.NotConnected();
            }
            else
            {
                _door.DoorLock();
                _oldId = ID;
                _state = LadeskabState.Locked;
                _display.scanRfid();
                _LogFile.logDoorLocked(ID.ToString());
                _charger.StartCharge();
            }
        }

        private void LockStateCheck(int ID)
        {
            if (!CheckID(ID))
            {
                _display.rfidError();
            }
            else
            {
                _door.DoorUnlock();
                _display.removePhone();
                _state = LadeskabState.Available;
                _charger.StopCharge();
                _LogFile.logDoorUnlocked(ID.ToString());
            }
        }

        private void CheckStateDoor()
        {
            if (_doorEvent == DoorState.DoorUnlock)
            {
                DoorOpen();
            }
            else
            {
                DoorClose();
            }
        }




    }
}
