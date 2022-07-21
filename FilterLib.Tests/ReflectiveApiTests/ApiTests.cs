using NUnit.Framework;
using System.Linq;

namespace FilterLib.Tests.ReflectiveApiTests
{
    public class ApiTests
    {
        [Test]
        public void TestListFilters() =>
            Assert.AreEqual(66, ReflectiveApi.GetFilterTypes().Count());

        [Test]
        public void TestListBlends() =>
            Assert.AreEqual(26, ReflectiveApi.GetBlendTypes().Count());

    }
}
