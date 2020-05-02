using Microsoft.VisualStudio.TestTools.UnitTesting;
using WebAPI_Client_Assistant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAPI_Client_Assistant.Tests
{
    [TestClass()]
    public class PersonWindowTests
    {
        [TestMethod()]
        public void ValidateSocialSecurityNumberTest()
        {
            PersonWindow pw = new PersonWindow(null);
            bool result = pw.ValidateSocialSecurityNumber("123 456 789");
            Assert.AreEqual(true, result);
        }

        [TestMethod()]
        public void ValidateDateTest()
        {
            PersonWindow pw = new PersonWindow(null);
            bool result = pw.ValidateDate("2020-02-02 08:00");
            Assert.AreEqual(true, result);
        }

        [TestMethod()]
        public void ValidateNameTest()
        {
            PersonWindow pw = new PersonWindow(null);
            bool result = pw.ValidateName("Jack Gates");
            Assert.AreEqual(true, result);
        }
    }
}