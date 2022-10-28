using Library.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Library.UtiAndSim;
using NUnit.Framework;

namespace Test
{
    public class TestDisplay
    {
        private IDisplay _uut;
        private StringWriter sw;

        [SetUp]
        public void Setup()
        {
            _uut = new Display();
            sw = new StringWriter();
            Console.SetOut(sw);
        }

        [Test]
        public void connectPhoneTest()
        {
            _uut.connectPhone();
            Assert.That(sw.ToString(), Contains.Substring("Please connect your phone"));
        }

        [Test]
        public void scanRfidTest()
        {
            _uut.scanRfid();
            Assert.That(sw.ToString(), Contains.Substring("Please scan your Rfid"));
        }

        [Test]
        public void connectionErrorTest()
        {
            _uut.connectionError();
            Assert.That(sw.ToString(), Contains.Substring("Try reconnecting your phone"));
        }

        [Test]
        public void occupiedTest()
        {
            _uut.occupied();
            Assert.That(sw.ToString(), Contains.Substring("The box is occupied"));
        }

        [Test]
        public void rfidErrorTest()
        {
            _uut.rfidError();
            Assert.That(sw.ToString(), Contains.Substring("Try scanning again"));
        }

        [Test]
        public void removePhoneTest()
        {
            _uut.removePhone();
            Assert.That(sw.ToString(), Contains.Substring("Please remove your phone"));
        }

        [Test]
        public void FullChargeTest()
        {
            _uut.FullCharge();
            Assert.That(sw.ToString(), Contains.Substring("Phone is fully charged"));
        }

        [Test]
        public void OverloadChargeTest()
        {
            _uut.OverCharged();
            Assert.That(sw.ToString(), Contains.Substring("Something is wrong, disconnect charger"));
        }

        [Test]
        public void NormalChargeTest()
        {
            _uut.NormalCharge();
            Assert.That(sw.ToString(), Contains.Substring("Charging is working as intended"));
        }

        [Test]
        public void NotConnectedTest()
        {
            _uut.NotConnected();
            Assert.That(sw.ToString(), Contains.Substring("Phone is not connected, check connection"));
        }

    }
}
