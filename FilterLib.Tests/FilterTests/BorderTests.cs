﻿using FilterLib.Filters.Border;
using FilterLib.Util;
using NUnit.Framework;

namespace FilterLib.Tests.FilterTests
{
    public class BorderTests
    {
        [Test]
        public void TestJitterBorder()
        {
            Assert.IsTrue(Common.CheckFilter("_input.bmp", "_input.bmp",
                new JitterBorderFilter(Size.Absolute(0), new RGB(0, 0, 0), 0), 1));
            Assert.IsTrue(Common.CheckFilter("_input.bmp", "JitterBorder_20_Red_0.bmp",
                new JitterBorderFilter(Size.Absolute(20), new RGB(255, 0, 0), 0), 1));
        }

        [Test]
        public void TestFadeBorder()
        {
            Assert.IsTrue(Common.CheckFilter("_input.bmp", "_input.bmp",
                new FadeBorderFilter(Size.Absolute(0), new RGB(0, 0, 0)), 1));
            Assert.IsTrue(Common.CheckFilter("_input.bmp", "FadeBorder_20_Red.bmp",
                new FadeBorderFilter(Size.Absolute(20), new RGB(255, 0, 0)), 1));
        }

        [Test]
        public void TestVignette()
        {
            Assert.IsTrue(Common.CheckFilter("_input.bmp", "_input.bmp", new VignetteFilter(Size.Relative(3), Size.Relative(2)), 1));
            Assert.IsTrue(Common.CheckFilter("_input.bmp", "Vignette_150pct_60pct.bmp", new VignetteFilter(Size.Relative(1.5f), Size.Relative(0.6f)), 1));
            Assert.IsTrue(Common.CheckFilter("_input.bmp", "Vignette_150pct_60pct.bmp", new VignetteFilter(Size.Absolute(120), Size.Absolute(48)), 1));
            Assert.IsTrue(Common.CheckFilter("_input.bmp", "Vignette_100pct_90pct.bmp", new VignetteFilter(Size.Relative(1f), Size.Relative(0.9f)), 1));
        }
    }
}
