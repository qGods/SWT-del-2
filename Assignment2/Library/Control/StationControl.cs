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

        // Her mangler flere member variable
        public LadeskabState _state { get; private set; }
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
            switch (_state)
            {
                case LadeskabState.DoorOpen:
                    
                    _display.connectPhone();
                break;

                case LadeskabState.Locked:
                     _display.scanRfid();
                break;
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
                        _door.DoorLock();
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

       

        // Her mangler de andre trigger handlere
    }
}
