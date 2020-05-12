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
            Assert.AreEqual(false, result);
        }

        [TestMethod()]
        public void ValidateNameTest()
        {
            PersonWindow pw = new PersonWindow(null);
            bool result = pw.ValidateName("Jack Gates");
            Assert.AreEqual(true, result);
        }

        [DataRow("123 123 12b", false)]
        [DataRow("123  12b", false)]
        [DataRow("123 123 126 ", false)]
        [DataRow("123 66 6666", false)]
        [DataRow("123 123 123", true)]
        [DataTestMethod]
        public void ValidateSSN(string arg, bool expected)
        {
            PersonWindow pw = new PersonWindow(null);
            bool result = pw.ValidateSocialSecurityNumber(arg);
            Assert.AreEqual(expected, result);
        }

        [DataRow("J j", false)]
        [DataRow("Jack ", false)]
        [DataRow("J J ", false)]
        [DataRow("JaCk", false)]
        [DataRow("23245", false)]
        [DataRow("Jack!", false)]
        [DataRow("Jack And Jill", true)]
        [DataTestMethod]
        public void ValidateName1(string arg, bool expected)
        {
            PersonWindow pw = new PersonWindow(null);
            bool result = pw.ValidateName(arg);
            Assert.AreEqual(expected, result);
        }

        [DataRow("1998-11-04", false)]
        [DataRow("1998:11:04 09:00", false)]
        [DataRow("1998", false)]
        [DataRow("abc abc", false)]
        [DataRow("1998-24-24 09:00", false)]
        [DataRow("1998-11-04 40:04", false)]
        [DataRow("1998-11-04 08:00", false)]
        [DataRow("2020-11-04 08:00", true)]
        [DataTestMethod]
        public void ValidateDate1(string arg, bool expected)
        {
            PersonWindow pw = new PersonWindow(null);
            bool result = pw.ValidateDate(arg);
            Assert.AreEqual(expected, result);
        }

        [DataTestMethod]
        [DynamicData(nameof(GetData),DynamicDataSourceType.Method)]
        public void ValidateSSN2(string arg, bool expected)
        {
            PersonWindow pw = new PersonWindow(null);
            bool result = pw.ValidateSocialSecurityNumber(arg);
            Assert.AreEqual(expected, result);
        }

        public static IEnumerable<object[]> GetData()
        {
            yield return new object[] { "245 255  332", false };
            yield return new object[] { " 255 332", false };
            yield return new object[] { "245 abc 332", false };
            yield return new object[] { "111 ", false };
            yield return new object[] { " ", false };
            yield return new object[] { "123 123 123", true };
        }

        [DataTestMethod]
        [DynamicData(nameof(GetData2), DynamicDataSourceType.Property)]
        public void ValidateName2(string arg, bool expected)
        {
            PersonWindow pw = new PersonWindow(null);
            bool result = pw.ValidateName(arg);
            Assert.AreEqual(expected, result);
        }

        public static IEnumerable<object[]> GetData2
        {
            get
            {
                yield return new object[] { "Matyas KIRALY", false };
                yield return new object[] { "Nincs Igazsag", true };
                yield return new object[] { "D N ", false };
                yield return new object[] { "FF D", false };
                yield return new object[] { "Igen Nem", true };
                yield return new object[] { "ddd dd", false };
            }
        }

        public TestContext TestContext { get; set; }

        [DataTestMethod]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV",
            @"Data\dates.csv",
            "dates#csv", DataAccessMethod.Sequential)]
        public void ValidateDate2()
        {
            PersonWindow pw = new PersonWindow(null);
            string arg = Convert.ToString(TestContext.DataRow["Date"].ToString());
            bool expected = Convert.ToBoolean(TestContext.DataRow["Expected"].ToString());
            bool result = pw.ValidateDate(arg);
            Assert.AreEqual(expected, result);
        }
    }
}