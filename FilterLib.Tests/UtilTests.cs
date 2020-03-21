using NUnit.Framework;
using FilterLib.Util;

namespace FilterLib.Tests
{
    public class UtilTests
    {
        [Test]
        public void TestClamp()
        {
            Assert.AreEqual(0, (-1000).Clamp(0, 255));
            Assert.AreEqual(0, (-1).Clamp(0, 255));
            Assert.AreEqual(0, 0.Clamp(0, 255));
            Assert.AreEqual(1, 1.Clamp(0, 255));
            Assert.AreEqual(25, 25.Clamp(0, 255));
            Assert.AreEqual(127, 127.Clamp(0, 255));
            Assert.AreEqual(254, 254.Clamp(0, 255));
            Assert.AreEqual(255, 255.Clamp(0, 255));
            Assert.AreEqual(255, 256.Clamp(0, 255));
            Assert.AreEqual(255, 1000.Clamp(0, 255));
        }
    }
}