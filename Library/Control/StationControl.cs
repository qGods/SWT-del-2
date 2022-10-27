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
        private enum LadeskabState //make private later
        {
            Available,
            Locked,
            DoorOpen
        };

        // Her mangler flere member variable
        private LadeskabState _state;
        private IChargingControl _charger;
        private int _oldId;
        private IDoor _door;
        private IDisplay _display;
        private IrfIDReader _rfIDReader;
        private ILogFile _LogFile;

        public DoorState _doorEvent { get; private set; }
        public int _rfIDEvent { get; private set;}

        private string logFile = "logfile.txt"; // Navnet på systemets log-fil

        // Her mangler constructor
        public StationControl(IChargingControl charger, IDoor door, IDisplay display, IrfIDReader rfIDReader, ILogFile LogFile)
        {
            door.DoorStateEvent += HandleDoorStateEvent;

            rfIDReader.rfIDEvent += RfidDetected; // subscribe with an event from rfIDReader

            _door = door;

            _charger = charger;

            _display = display;

            _LogFile = LogFile;

            _rfIDReader = rfIDReader;

            _state = LadeskabState.Available;
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
            if (_doorEvent == DoorState.DoorUnlock())
            {
                DoorOpen();
            }
            else
            {
                DoorClose();
            }
        }

        // Eksempel på event handler for eventet "RFID Detected" fra tilstandsdiagrammet for klassen
        private void RfidDetected(object sender, rfIDDetectedArgs e)
        {
            switch (_state)
            {
                case LadeskabState.Available:
                    // Check for ladeforbindelse
                    if (_charger.IsConnected)
                    {
                        _door.DoorLock(); //should already be locked
                        _charger.StartCharge();
                        _oldId = e.rfIDDetected;
                        using (var writer = File.AppendText(logFile))
                        {
                            writer.WriteLine(DateTime.Now + ": Skab låst med RFID: {0}", e.rfIDDetected);
                        }

                        Console.WriteLine("Skabet er låst og din telefon lades. Brug dit RFID tag til at låse op.");
                        _state = LadeskabState.Locked;
                    }
                    else
                    {
                        Console.WriteLine("Din telefon er ikke ordentlig tilsluttet. Prøv igen.");
                    }

                    break;

                case LadeskabState.DoorOpen:
                    // Ignore
                    break;

                case LadeskabState.Locked:
                    // Check for correct ID
                    if (e.rfIDDetected == _oldId)
                    {
                        _charger.StopCharge();
                        _door.DoorUnlock();
                        using (var writer = File.AppendText(logFile))
                        {
                            writer.WriteLine(DateTime.Now + ": Skab låst op med RFID: {0}", e.rfIDDetected);
                        }

                        Console.WriteLine("Tag din telefon ud af skabet og luk døren");
                        _state = LadeskabState.Available;
                    }
                    else
                    {
                        Console.WriteLine("Forkert RFID tag");
                    }

                    break;
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

    }
}
