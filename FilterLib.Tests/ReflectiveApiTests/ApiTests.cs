using NUnit.Framework;
using System.Linq;

namespace FilterLib.Tests.ReflectiveApiTests
{
    public class ApiTests
    {
        [Test]
        public void TestListFilters() =>
            Assert.AreEqual(65, ReflectiveApi.GetFilterTypes().Count());

        [Test]
        public void TestListBlends() =>
            Assert.AreEqual(25, ReflectiveApi.GetBlendTypes().Count());

    }
}
