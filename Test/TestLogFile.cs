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
    public class TestLogFile
    {
        private ILogFile _uut;
    
        [SetUp]
        public void Setup()
        {
            _uut = new LogFile();
            
        }

        [Test]
        public void logDoorLockedTest()
        {
            int id = 1;
            _uut.logDoorLocked(id.ToString());
            string message = DateTime.Now + "ID: {0} has locked the door";
            Assert.That(id, message) ;
        }
    
    }

    

}
