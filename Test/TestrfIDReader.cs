using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Library.Interface;
using Library.UtiAndSim;
using NUnit.Framework;

namespace Test
{
    public class TestrfIDReader
    {
        private IrfIDReader _uut;
        private rfIDDetectedArgs _uutEvent;

        [SetUp]
        public void Setup()
        {
            _uut = new rfIDReader();
            _uut.rfIDEvent += (o, args) => 
            { 
                _uutEvent = args; 
            };
        }

        
        [TestCase(0)]
        [TestCase(10)]
        public void rfIDTest(int id)
        {
            _uut.ReadrfID(id);
            Assert.That(_uutEvent.rfIDDetected, Is.EqualTo(id));
        }
    }
}
