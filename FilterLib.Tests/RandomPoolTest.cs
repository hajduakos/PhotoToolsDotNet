using FilterLib.Util;
using NUnit.Framework;

namespace FilterLib.Tests;

public class RandomPoolTest
{
    [Test]
    public void TestSameSizeAndSeedGivesSameNumbers()
    {
        const int size = 8;
        const int seed = 123;
        RandomPool pool1 = new(size, seed);
        RandomPool pool2 = new(size, seed);

        for (int i = 0; i < size; i++)
            for (int j = 0; j < 10; j++)
                Assert.That(pool1[i].Next(), Is.EqualTo(pool2[i].Next()));
    }
}
