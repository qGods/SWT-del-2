using Library.Control;
using Library.UtiAndSim;
using Library.Interface;
class Program
{
    static void Main(string[] args)
    {
        IDoor door = new Door();
        IUsbCharger charger = new UsbChargerSimulator();
        IDisplay display = new Display();
        IChargingControl chargecontrol = new ChargingControl(charger,display);
        IrfIDReader rfidreader = new rfIDReader();
        ILogFile logfile = new LogFile();
        StationControl control = new StationControl(chargecontrol,door,display,rfidreader,logfile);

            // Assemble your system here from all the classes
        bool finish = false;
        do
        {
            
            //hej
            
            string input;
            System.Console.WriteLine("Indtast E, O, C, R: ");
            input = Console.ReadLine();
            if (string.IsNullOrEmpty(input)) continue;

            switch (input[0])
            {
                case 'E':
                    finish = true;
                    break;

                case 'O':
                    //door.OnDoorOpen();
                    door.DoorLock();
                    break;

                case 'C':
                    //door.OnDoorClose();
                    break;

                case 'R':
                    System.Console.WriteLine("Indtast RFID id: ");
                    string idString = System.Console.ReadLine();

                    int id = Convert.ToInt32(idString);
                    //rfidReader.OnRfidRead(id);
                    break;

                default:
                    break;
            }

        } while (!finish);
    }
}
