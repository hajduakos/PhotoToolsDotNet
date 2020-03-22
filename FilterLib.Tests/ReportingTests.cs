﻿using FilterLib.Reporting;
using NUnit.Framework;
using System.Collections.Generic;

namespace FilterLib.Tests
{
    public class ReportingTests
    {
        private class ReporterStub : IReporter
        {
            public List<int> Reports { get;  private set; }

            public ReporterStub() => Reports = new List<int>();

            public void Done() { }

            public void Report(int value, int min = 0, int max = 100) => Reports.Add(value);

            public void Start() { }
        }

        [Test]
        public void TestSubReporter()
        {
            // Main reporter
            ReporterStub rs = new ReporterStub();
            // Subreporter for 0-50 within 0-100
            IReporter sub = new SubReporter(rs, 0, 50, 0, 100);
            sub.Start();
            sub.Report(500, 0, 1000); // Middle will be 25
            sub.Done();
            // Subreporter for 50-100 within 0-100
            sub = new SubReporter(rs, 50, 100, 0, 100);
            sub.Start();
            sub.Report(10, 0, 20); // Middle will be 75
            sub.Done();
            Assert.AreEqual(new List<int> {0, 25, 50, 50, 75, 100 }, rs.Reports);
        }

        [Test]
        public void TestNestedSubReporters()
        {
            // Main reporter
            ReporterStub rs = new ReporterStub();
            // Subreporter for 0-500 within 0-1000
            IReporter sub = new SubReporter(rs, 0, 500, 0, 1000);
            // Sub-sub reporter for 500-1000
            IReporter sub2 = new SubReporter(sub, 500, 1000, 0, 1000);
            sub2.Report(500, 0, 1000); // Report into middle --> 37.5
            Assert.AreEqual(new List<int> { 375 }, rs.Reports);

        }
    }
}
