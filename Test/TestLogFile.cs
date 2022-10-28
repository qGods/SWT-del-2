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
            var list = new List<string>();
            var fileStream = new FileStream(@".\LogFile.txt", FileMode.Open, FileAccess.Read);

            using (var streamReader = new StreamReader(fileStream)
            {
                while(streamReader.Peek != -1)
                {
                    list.Add(streamReader.ReadLine());
                }
            }
            var result = list[List.Count - 2] + list[list.Count - 1];
            string message = DateTime.Now + "\nID: {0} has locked the door", id;
            Assert.AreEqual<string>(result, message) ;
        }


    
    }

    

}
