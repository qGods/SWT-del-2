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
        public void scanRfid()
        {
            _uut.connectPhone();
            Assert.That(sw.ToString(), Contains.Substring("Please scan your Rfid"));
        }

        [Test]
        public void connectionError()
        {
            _uut.connectPhone();
            Assert.That(sw.ToString(), Contains.Substring("Try reconnecting your phone"));
        }


        public void occupied()
        {
            _uut.connectPhone();
            Assert.That(sw.ToString(), Contains.Substring("The box is occupied"));
        }

        public void rfidError()
        {
            _uut.connectPhone();
            Assert.That(sw.ToString(), Contains.Substring("Try scanning again"));
        }


        public void removePhone()
        {
            _uut.connectPhone();
            Assert.That(sw.ToString(), Contains.Substring("Please remove your phone"));
        }

        public void FullCharge()
        {
            _uut.connectPhone();
            Assert.That(sw.ToString(), Contains.Substring("Phone is fully charged"));
        }

        public void OverloadCharge()
        {
            _uut.connectPhone();
            Assert.That(sw.ToString(), Contains.Substring("Something is wrong, disconnect charger"));
        }

        public void NormalCharge()
        {
            _uut.connectPhone();
            Assert.That(sw.ToString(), Contains.Substring("Charging is working as intended"));
        }

        public void OverCharged()
        {
            _uut.connectPhone();
            Assert.That(sw.ToString(), Contains.Substring("Phone is over charged, remove charger immediately"));
        }

        public void NotConnected()
        {
            _uut.connectPhone();
            Assert.That(sw.ToString(), Contains.Substring("Phone is not connected, check connection"));
        }

    }
}
