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
    public class StationControl: IStationControl
    {
        // Enum med tilstande ("states") svarende til tilstandsdiagrammet for klassen
        public enum LadeskabState
        {
            Available,
            Locked,
            DoorOpen
        };

        public  LadeskabState _state { get; set; }
        private IChargingControl _charger;
        public int _oldID { get; private set; }
        private IDoor _door;
        private IDisplay _display;
        private IrfIDReader _rfIDReader;
        private ILogFile _LogFile;
        public DoorState _doorEvent { get; private set; }
        public int _rfIDEvent { get; private set;}
        private string logFile = "logfile.txt";


        public StationControl(IChargingControl charger, IDoor door, IrfIDReader rfIDReader, IDisplay display, ILogFile logFile)
        {
            door.DoorStateEvent += HandleDoorStateEvent;

            _rfIDReader.rfIDEvent += RfidDetected;

            _door = door;

            _charger = charger;

            _display = display;

            _LogFile = logFile;

            _rfIDReader = rfIDReader;

            _state = LadeskabState.Available;
        }

        public void RfidDetected(object? rfIDReader, rfIDDetectedArgs rfidArgs)
        {
            if (_state == LadeskabState.Available)
            {
                if (!_charger.IsConnected)
                {
                    _display.NotConnected();
                    using (var writer = File.AppendText(logFile))
                    {
                        writer.WriteLine(DateTime.Now + "Phone Not Connected", rfidArgs.ID);
                    }
                }

                _charger.StartCharge();

                _door.DoorLock();
                _oldID = rfidArgs.ID;

                _LogFile.logDoorLocked(_oldID);

                _state = LadeskabState.Locked;
            }

            else if (_state == LadeskabState.Locked)
                {
                    if (CheckID(rfidArgs.ID))
                    {
                        _charger.StopCharge();
                        _door.DoorUnlock();
                        _LogFile.logDoorUnlocked(rfidArgs.ID);
                        _state = LadeskabState.Available;
                        _display.removePhone();
                    }
                    else
                    {
                        _display.occupied();
                    }
                }
        }

        private bool CheckID(int ID)
        {
            if (ID == _oldID)
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

        private void AvailableState(object? rfidReader, rfIDDetectedArgs rfidArgs)
        {
            if (!IsConnected())
            {
                _display.NotConnected();
            }
            else
            {
                _door.DoorLock();
                _oldID = rfidArgs.ID;
                _state = LadeskabState.Locked;
                _display.scanRfid();
                _LogFile.logDoorLocked(_oldID);
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
                _LogFile.logDoorUnlocked(_oldID);
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
