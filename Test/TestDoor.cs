using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Library.Interface;
using Library.UtiAndSim;
using NUnit.Framework;

namespace Test
{
    public class TestDoor
    {
        private IDoor _uut;
        private DoorStateEventArgs _open;

        [SetUp]
        public void Setup()
        {
            _open = null;
            _uut = new Door();

            _uut.DoorStateEvent += (o, args) =>
            {
                _open = args;
            };
        }

        [Test]
        public void DoorLockTest()
        {
            _uut.DoorLock();
            Assert.That(_uut.locked== false);
        }

        [Test]
        public void DoorUnlockTest()
        {
            _uut.DoorUnlock();
            Assert.That(_uut.locked == false);
        }

        [Test]
        public void DoorOpenTest()
        {
            _uut.DoorOpened();
            Assert.That(_uut._open == true);
        }

        [Test]
        public void DoorCloseTest()
        {
            _uut.DoorClosed();
            Assert.That(_uut._open == false);
        }

        [Test]

        public void DoorStateEventTest() //Event test opened
        {
            _uut.DoorOpened();
            Assert.That(_open.DoorStateEvent, Is.EqualTo(DoorState.open));
        }


    }
}
