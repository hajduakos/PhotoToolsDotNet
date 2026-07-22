using FilterLib.Filters;
using NUnit.Framework;
using System.Linq;

namespace FilterLib.Tests.ReflectiveApiTests;

public class ApiTests
{
    [Test]
    public void TestListFilters() =>
        Assert.That(ReflectiveApi.GetFilterTypes().Count(), Is.EqualTo(81));

    [Test]
    public void TestListBlends() =>
        Assert.That(ReflectiveApi.GetBlendTypes().Count(), Is.EqualTo(26));

    [Test]
    public void TestFilterExists() =>
        Assert.That(ReflectiveApi.CheckFilterExists(typeof(Filters.Color.InvertFilter).Name));

    [Test]
    public void TestFilterNotExists() =>
        Assert.That(ReflectiveApi.CheckFilterExists("This should not exist..."), Is.False);

    [Test]
    public void TestBlendExists() =>
        Assert.That(ReflectiveApi.CheckBlendExists(typeof(Blending.Normal.NormalBlend).Name));

    [Test]
    public void TestBlendNotExists() =>
        Assert.That(ReflectiveApi.CheckBlendExists("This should not exist..."), Is.False);

    [Test]
    public void TestFilterDescriptions()
    {
        foreach (var type in ReflectiveApi.GetFilterTypes())
        {
            var attr = (FilterAttribute)type.GetCustomAttributes(typeof(FilterAttribute), false).Single();
            Assert.That(attr.Description, Is.Not.Empty, $"Filter '{type.Name}' has no description.");
        }
    }

    [Test]
    public void TestBlendDescriptions()
    {
        foreach (var type in ReflectiveApi.GetBlendTypes())
        {
            var attr = (Blending.BlendAttribute)type.GetCustomAttributes(typeof(Blending.BlendAttribute), false).Single();
            Assert.That(attr.Description, Is.Not.Empty, $"Blend '{type.Name}' has no description.");
        }
    }

    [Test]
    public void TestFilterNotExistsException() =>
        Assert.Throws<System.ArgumentException>(() => ReflectiveApi.ConstructFilterByName("This should not exist..."));

    [Test]
    public void TestBlendNotExistsException() =>
        Assert.Throws<System.ArgumentException>(() => ReflectiveApi.ConstructBlendByName("This should not exist..."));

}
