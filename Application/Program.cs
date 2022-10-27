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
        IrfIDReader rfidReader = new rfIDReader();
        ILogFile logfile = new LogFile();
        StationControl control = new StationControl(chargecontrol,door,display,rfidReader,logfile);
        // Assemble your system here from all the classes
        bool finish = false;
        do
        {
            string input;
            System.Console.WriteLine("Indtast E, O, C, R: ");
            input = Console.ReadLine().ToUpper();
            if (string.IsNullOrEmpty(input)) continue;

            switch (input[0])
            {
                case 'E':
                    finish = true;
                    break;

                case 'O':
                    door.DoorUnlock();
                    break;

                case 'C':
                    door.DoorLock();
                    break;

                case 'R':
                    System.Console.WriteLine("Indtast RFID id: ");
                    string idString = System.Console.ReadLine();
                    int id = Convert.ToInt32(idString);
                    rfidReader.ReadrfID(id);
                    break;

                default:
                    break;
            }

        } while (!finish);
    }
}
