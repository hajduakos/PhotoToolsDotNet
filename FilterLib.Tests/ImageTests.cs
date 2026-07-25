using FilterLib.Util;
using NUnit.Framework;

namespace FilterLib.Tests;

[TestFixture]
[Parallelizable(ParallelScope.All)]
public class ImageTests
{
    [Test]
    public void TestWidthAndHeight()
    {
        Image img = new(4, 7);
        Assert.That(img.Width, Is.EqualTo(4));
        Assert.That(img.Height, Is.EqualTo(7));
    }

    [Test]
    public void TestConstructorDefaultColorFillsEveryPixel()
    {
        RGB color = new(10, 20, 30);
        Image img = new(3, 2, color);
        for (int y = 0; y < img.Height; ++y)
            for (int x = 0; x < img.Width; ++x)
            {
                Assert.That(img[x, y, 0], Is.EqualTo(color.R), $"Pixel ({x}, {y}) red mismatch.");
                Assert.That(img[x, y, 1], Is.EqualTo(color.G), $"Pixel ({x}, {y}) green mismatch.");
                Assert.That(img[x, y, 2], Is.EqualTo(color.B), $"Pixel ({x}, {y}) blue mismatch.");
            }
    }

    [Test]
    public void TestConstructorDefaultsToBlack()
    {
        Image img = new(3, 2);
        for (int y = 0; y < img.Height; ++y)
            for (int x = 0; x < img.Width; ++x)
                for (int c = 0; c < 3; ++c)
                    Assert.That(img[x, y, c], Is.EqualTo(0), $"Pixel ({x}, {y}) component {c} is not black.");
    }

    [Test]
    public void TestIndexerGetSetRoundTrip()
    {
        Image img = new(3, 2);
        // Set every component of every pixel to a distinct value
        for (int y = 0; y < img.Height; ++y)
            for (int x = 0; x < img.Width; ++x)
                for (int c = 0; c < 3; ++c)
                    img[x, y, c] = (byte)(x * 40 + y * 10 + c);
        // Read back and verify each value round-trips independently
        for (int y = 0; y < img.Height; ++y)
            for (int x = 0; x < img.Width; ++x)
                for (int c = 0; c < 3; ++c)
                    Assert.That(img[x, y, c], Is.EqualTo((byte)(x * 40 + y * 10 + c)),
                        $"Pixel ({x}, {y}) component {c} did not round-trip.");
    }

    [Test]
    public void TestCloneIsIndependent()
    {
        // Create a small blank black image
        Image original = new(3, 2);

        // Clone and change one pixel to white
        Image clone = (Image)original.Clone();
        clone[1, 1, 0] = clone[1, 1, 1] = clone[1, 1, 2] = 255;

        // The clone must have the changed pixel
        Assert.That(clone[1, 1, 0], Is.EqualTo(255));
        Assert.That(clone[1, 1, 1], Is.EqualTo(255));
        Assert.That(clone[1, 1, 2], Is.EqualTo(255));

        // The original must remain fully black
        for (int y = 0; y < original.Height; ++y)
            for (int x = 0; x < original.Width; ++x)
                for (int c = 0; c < 3; ++c)
                    Assert.That(original[x, y, c], Is.EqualTo(0),
                        $"Original pixel ({x}, {y}) component {c} was modified.");
    }
}
