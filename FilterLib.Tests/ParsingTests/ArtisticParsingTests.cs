﻿using FilterLib.Filters.Artistic;
using FilterLib.Scripting;
using NUnit.Framework;

namespace FilterLib.Tests.ParsingTests
{
    public class ArtisticParsingTests
    {
        [Test]
        public void TestAdaptiveTresholdParse()
        {
            var filters = Parser.Parse(new string[] { "AdaptiveTreshold" });
            Assert.AreEqual(1, filters.Count);
            Assert.IsInstanceOf<AdaptiveTresholdFilter>(filters[0]);

            filters = Parser.Parse(new string[] { "AdaptiveTreshold", "- SquareSize: 8" });
            Assert.AreEqual(1, filters.Count);
            Assert.IsInstanceOf<AdaptiveTresholdFilter>(filters[0]);
            AdaptiveTresholdFilter f = filters[0] as AdaptiveTresholdFilter;
            Assert.AreEqual(8, f.SquareSize);
        }

        [Test]
        public void TestOilPaintParse()
        {
            var filters = Parser.Parse(new string[] { "OilPaint" });
            Assert.AreEqual(1, filters.Count);
            Assert.IsInstanceOf<OilPaintFilter>(filters[0]);

            filters = Parser.Parse(new string[] { "OilPaint", "- Radius: 8" });
            Assert.AreEqual(1, filters.Count);
            Assert.IsInstanceOf<OilPaintFilter>(filters[0]);
            OilPaintFilter f = filters[0] as OilPaintFilter;
            Assert.AreEqual(8, f.Radius);
        }

        [Test]
        public void TestRandomJitterParse()
        {
            var filters = Parser.Parse(new string[] { "RandomJitter" });
            Assert.AreEqual(1, filters.Count);
            Assert.IsInstanceOf<RandomJitterFilter>(filters[0]);

            filters = Parser.Parse(new string[] { "RandomJitter", "- Radius: 8", "- Seed: 1000" });
            Assert.AreEqual(1, filters.Count);
            Assert.IsInstanceOf<RandomJitterFilter>(filters[0]);
            RandomJitterFilter f = filters[0] as RandomJitterFilter;
            Assert.AreEqual(8, f.Radius);
            Assert.AreEqual(1000, f.Seed);
        }
    }
}