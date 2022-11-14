using Library.Control;
using Library.Interface;
using NSubstitute;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test
{
    internal class TestChargeControl
    {
        private IChargingControl _uut;
        private IUsbCharger _usbCharger;
        private IDisplay _display;

        [SetUp]
        public void Setup()
        {
            _usbCharger = Substitute.For<IUsbCharger>();
            _display = Substitute.For<IDisplay>();
            _uut = new ChargingControl(_usbCharger, _display);

        }
        //Tests that starting charging control starts the charger and connected it. 
        [Test]
        public void StartChargeTest()
        {
            _uut.StartCharge();
            _usbCharger.Received(1).StartCharge(); //did the unit recieve the correct function call 1 time
            Assert.That(_uut.IsConnected, Is.True);
        }

        //Ensures stopping the uut, stops the charger. 
        [Test]
        public void StopChargeTest()
        {
            _uut.StopCharge();
            _usbCharger.Received(1).StopCharge();
        }
        //Tests if the charger is unconnected by default
        [Test]
        public void IsConnectedDefaultTest()
        {
            Assert.That(_uut.IsConnected, Is.False);
        }

        [Test]
        public void ConnectTest()
        {
            _usbCharger.StartCharge();
            Assert.That(_uut.IsConnected,Is.False); //charging control should start usb charger, not other way around, out of scope here. 
        }

        //Tests if uut gets updated current correctly on charger change. 
        [TestCase(501)]
        [TestCase(0)]
        public void OnNewCurrentTest(double val)
        {
            _usbCharger.CurrentValueEvent += Raise.EventWith(new CurrentEventArgs { Current = val });
            Assert.That(_uut.currentValue, Is.EqualTo(val));
        }
        [Test]

        /**
         * The following section of tests ensure the correct behaviour for differentiating values of current. 
         */
        //Both tests supposed to respond with Nonconnected does not return desired value. Unable to fix in timeframe. 
        public void ChargerIntervalsTest_NegativeMa()
        {
            _usbCharger.CurrentValueEvent += Raise.EventWith(new CurrentEventArgs { Current = -10 });
            _display.Received(1).NotConnected();

        }
        [Test]
        //No idea why this is failing
        public void ChargerIntervalsTest_00ma()
        {        
            _usbCharger.CurrentValueEvent += Raise.EventWith(new CurrentEventArgs { Current = 0 });
            _display.Received(1).NotConnected();
        }
        [Test]
        public void ChargerIntervalsTest_01ma()
        {
         _usbCharger.CurrentValueEvent += Raise.EventWith(new CurrentEventArgs { Current = 1 });
         _display.Received(1).FullCharge();


        }
        [Test]
        public void ChargerIntervalsTest_04ma()
        {
            _usbCharger.CurrentValueEvent += Raise.EventWith(new CurrentEventArgs { Current = 4 });
            _display.Received(1).FullCharge();

        }





        [Test]
        public void ChargerIntervalsTest_5ma()
        {  
            _usbCharger.CurrentValueEvent += Raise.EventWith(new CurrentEventArgs { Current = 5 });
            _display.Received(1).FullCharge();
        }
        [Test]
        public void ChargerIntervalsTest_6ma()
        {
            _usbCharger.CurrentValueEvent += Raise.EventWith(new CurrentEventArgs { Current = 6 });
            _display.Received(1).NormalCharge();
        }

        [Test]
        public void ChargerIntervalsTest_100ma()
        {
            _usbCharger.CurrentValueEvent += Raise.EventWith(new CurrentEventArgs { Current = 100 });
            _display.Received(1).NormalCharge();
        }
        [Test]
        public void ChargerIntervalsTest_499ma()
        {  
        }
        [Test]
        public void ChargerIntervalsTest_500ma()
        {  
        }
        
        [Test]
        public void ChargerIntervalsTest_501ma()
        {
            _usbCharger.CurrentValueEvent += Raise.EventWith(new CurrentEventArgs { Current = 501 });
            _usbCharger.Received(1).StopCharge();
            _display.Received(1).OverCharged();
        }
    }
}
