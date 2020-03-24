using FilterLib.Filters;
using NUnit.Framework;
using System;

namespace FilterLib.Tests.ReflectiveApiTests
{
    public class ExceptionTests
    {
        [Test]
        public void TestNonExistingFilter() =>
            Assert.Throws<ArgumentException>(() => ReflectiveApi.ConstructFilterByName("NoSuchFilter"));

        [Test]
        public void TestNonExistingParam()
        {
            IFilter f = ReflectiveApi.ConstructFilterByName("Brightness");
            Assert.Throws<ArgumentException>(() => ReflectiveApi.SetFilterPropertyByName(f, "NoSuchProperty", "0"));
        }

        [Test]
        public void TestParamInvalidValue()
        {
            IFilter f = ReflectiveApi.ConstructFilterByName("Brightness");
            Assert.Throws<FormatException>(() => ReflectiveApi.SetFilterPropertyByName(f, "Brightness", "abc"));
        }

        [Test]
        public void TestParamGreater()
        {
            IFilter f = ReflectiveApi.ConstructFilterByName("Brightness");
            Assert.Throws<ArgumentOutOfRangeException>(() => ReflectiveApi.SetFilterPropertyByName(f, "Brightness", "256"));
        }

        [Test]
        public void TestParamLess()
        {
            IFilter f = ReflectiveApi.ConstructFilterByName("Brightness");
            Assert.Throws<ArgumentOutOfRangeException>(() => ReflectiveApi.SetFilterPropertyByName(f, "Brightness", "-256"));
        }

        [Test]
        public void TestParamLessF()
        {
            IFilter f = ReflectiveApi.ConstructFilterByName("Gamma");
            Assert.Throws<ArgumentOutOfRangeException>(() => ReflectiveApi.SetFilterPropertyByName(f, "Gamma", "-0.1"));
        }
    }
}
