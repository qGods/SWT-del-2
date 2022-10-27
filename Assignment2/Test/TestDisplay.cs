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
    }
}
