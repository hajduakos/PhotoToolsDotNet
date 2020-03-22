using FilterLib.Scripting;
using NUnit.Framework;

namespace FilterLib.Tests.ParsingTests
{
    public class ExceptionTests
    {

        [Test]
        public void TestParamBeforeFilter() =>
            Assert.Throws<SyntaxException>(() => Parser.Parse(new string[] { "- Brightness: 50", "Brightness" }));

        [Test]
        public void TestNonExistingFilter() =>
            Assert.Throws<FilterNotFoundException>(() => Parser.Parse(new string[] { "NoSuchFilter" }));

        [Test]
        public void TestNonExistingParam() =>
            Assert.Throws<ParamNotFoundException>(() => Parser.Parse(new string[] { "Brightness", "- NoSuchParam: 0" }));

        [Test]
        public void TestParamInvalidSyntax() =>
            Assert.Throws<SyntaxException>(() => Parser.Parse(new string[] { "Brightness", "- Brightness" }));

        [Test]
        public void TestParamInvalidValue() =>
            Assert.Throws<ParseException>(() => Parser.Parse(new string[] { "Brightness", "- Brightness: abc" }));
    }
}
