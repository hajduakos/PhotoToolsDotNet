using FilterLib.Filters;
using FilterLib.Filters.Generate;
using NUnit.Framework;

namespace FilterLib.Tests.ReflectiveApiTests
{
    public class GenerateApiTests
    {

        [Test]
        public void TestMarble()
        {
            IFilter f = ReflectiveApi.ConstructFilterByName("Marble");
            Assert.IsInstanceOf<MarbleFilter>(f);
            ReflectiveApi.SetFilterPropertyByName(f, "HorizontalLines", "1");
            ReflectiveApi.SetFilterPropertyByName(f, "VerticalLines", "2");
            ReflectiveApi.SetFilterPropertyByName(f, "Twist", "3");
            ReflectiveApi.SetFilterPropertyByName(f, "Iterations", "4");
            ReflectiveApi.SetFilterPropertyByName(f, "Seed", "5");
            MarbleFilter ff = f as MarbleFilter;
            Assert.AreEqual(1, ff.HorizontalLines);
            Assert.AreEqual(2, ff.VerticalLines);
            Assert.AreEqual(3, ff.Twist);
            Assert.AreEqual(4, ff.Iterations);
            Assert.AreEqual(5, ff.Seed);
        }
    }
}
