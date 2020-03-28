using NUnit.Framework;
using System.Linq;

namespace FilterLib.Tests.ReflectiveApiTests
{
    public class ApiTests
    {
        [Test]
        public void TestListFilters() =>
            Assert.AreEqual(50, ReflectiveApi.GetFilterTypes().ToArray().Length);

        [Test]
        public void TestListBlends() =>
            Assert.AreEqual(6, ReflectiveApi.GetBlendTypes().ToArray().Length);

    }
}
