using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Library.Control;
using Library.Interface;
using Library.UtiAndSim;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;
using NSubstitute;
using NuGet.Frameworks;
using NUnit.Framework;

namespace Test
{
    [TestFixture]
    public class TestStationControl
    {
        private StationControl _uut;
        private IrfIDReader _rfIDReader;
        private IDoor _door;
        private ILogFile _logFile;
        private IChargingControl _chargingControl;
        private IDisplay _display;

        [SetUp]
        public void SetUp()
        {
            // Arrange
            _rfIDReader = Substitute.For<IrfIDReader>();
            _door = Substitute.For<IDoor>();
            _logFile = Substitute.For<ILogFile>();
            _chargingControl = Substitute.For<IChargingControl>();
            _display = Substitute.For<IDisplay>();
            _uut = new StationControl(_chargingControl, _door,  _rfIDReader, _display, _logFile);
        }

        [Test]
        public void DoorState_Open()
        {
            _door.DoorStateEvent += Raise.EventWith(new DoorStateEventArgs() { DoorStateEvent = DoorState.open });

            Assert.That(_uut._doorEvent, Is.EqualTo(DoorState.open));
        }

        [Test]
        public void DoorState_Close()
        {
            _door.DoorStateEvent += Raise.EventWith(new DoorStateEventArgs() { DoorStateEvent = DoorState.closed });

            Assert.That(_uut._doorEvent, Is.EqualTo(DoorState.closed));
        }

        [Test]
        public void rfIDReaderDetected_Availabe()
        {
            _uut._state = StationControl.LadeskabState.Available;
            _chargingControl.IsConnected = false;
            _rfIDReader.rfIDEvent += Raise.EventWith(new rfIDDetectedArgs() { rfIDDetected = 1 });
            _chargingControl.Received(0).StartCharge();
            _door.Received(0).DoorLock();
            _logFile.Received(0).logDoorLocked('1');

            Assert.That(_uut._state, Is.EqualTo(StationControl.LadeskabState.Available));
        }

        [Test]
        public void rfIDReaderDetected2_Availabe()
        {
            _uut._state = StationControl.LadeskabState.Available;
            _chargingControl.IsConnected = false;
            _rfIDReader.rfIDEvent += Raise.EventWith(new rfIDDetectedArgs() { rfIDDetected = 1 });
            _chargingControl.Received(0).StartCharge();
            _door.Received(0).DoorLock();
            _logFile.Received(0).logDoorLocked('1');
            Assert.That(_uut._state, Is.EqualTo(StationControl.LadeskabState.Available));

            _chargingControl.IsConnected = true;
            _rfIDReader.rfIDEvent += Raise.EventWith(new rfIDDetectedArgs() { rfIDDetected = 1 });

            _chargingControl.Received(0).StartCharge();
            _door.Received(1).DoorLock();
            _logFile.Received(1).logDoorLocked('1');
            Assert.That(_uut._state, Is.EqualTo(StationControl.LadeskabState.Locked));
        }


        [Test]
        public void rfIDReaderDetected_Locked()
        {
            _uut._state = StationControl.LadeskabState.Locked;
            _chargingControl.IsConnected = true;
            _rfIDReader.rfIDEvent += Raise.EventWith(new rfIDDetectedArgs() { rfIDDetected = 2 });

            Assert.That(_uut._state, Is.EqualTo(StationControl.LadeskabState.Locked));
            Assert.That(_uut._rfIDEvent, Is.EqualTo(2));

            _rfIDReader.rfIDEvent += Raise.EventWith(new rfIDDetectedArgs() { rfIDDetected = 1 });
            _chargingControl.Received(0).StopCharge();
            _display.Received(1).rfidError();
            Assert.That(_uut._state, Is.EqualTo(StationControl.LadeskabState.Locked));

            _rfIDReader.rfIDEvent += Raise.EventWith(new rfIDDetectedArgs() { rfIDDetected = 2 });
            _chargingControl.Received(0).StopCharge();
            _door.Received(1).DoorLock();
            Assert.That(_uut._state, Is.EqualTo(StationControl.LadeskabState.Available));


            _door.DoorStateEvent += Raise.EventWith(new DoorStateEventArgs() { DoorStateEvent = DoorState.open });
            _rfIDReader.rfIDEvent += Raise.EventWith(new rfIDDetectedArgs() { rfIDDetected = 2 });
            Assert.That(_uut._state, Is.EqualTo(StationControl.LadeskabState.DoorOpen));



        }



    }
}
