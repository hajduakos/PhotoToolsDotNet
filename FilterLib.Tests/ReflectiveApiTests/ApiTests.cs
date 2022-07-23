﻿using NUnit.Framework;
using System.Linq;

namespace FilterLib.Tests.ReflectiveApiTests
{
    public class ApiTests
    {
        [Test]
        public void TestListFilters() =>
            Assert.AreEqual(69, ReflectiveApi.GetFilterTypes().Count());

        [Test]
        public void TestListBlends() =>
            Assert.AreEqual(26, ReflectiveApi.GetBlendTypes().Count());

        [Test]
        public void TestFilterExists() =>
            Assert.IsTrue(ReflectiveApi.CheckFilterExists(typeof(Filters.Color.InvertFilter).Name));

        [Test]
        public void TestFilterNotExists() =>
            Assert.IsFalse(ReflectiveApi.CheckFilterExists("This should not exist..."));

        [Test]
        public void TestBlendExists() =>
            Assert.IsTrue(ReflectiveApi.CheckBlendExists(typeof(Blending.Normal.NormalBlend).Name));

        [Test]
        public void TestBlendNotExists() =>
            Assert.IsFalse(ReflectiveApi.CheckBlendExists("This should not exist..."));

    }
}
