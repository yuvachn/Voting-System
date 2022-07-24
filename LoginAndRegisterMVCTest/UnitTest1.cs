using NUnit.Framework;

namespace LoginAndRegisterMVCTest
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Test1()
        {
            Assert.AreEqual(5,2+3);
        }

        [Test]
        public void CheckDepartmentExistWithMoq()
        {
            bool Res = true;
            Assert.That(Res, Is.True);
        }
    }
}