using FilterLib;
using FilterLib.Filters;
using FilterLib.Filters.Adjustments;
using FilterLib.Filters.Artistic;
using FilterLib.Filters.Blur;
using FilterLib.Filters.Border;
using FilterLib.Filters.Color;
using FilterLib.Filters.Dither;
using FilterLib.Filters.Edges;
using FilterLib.Filters.Generate;
using FilterLib.Filters.Mosaic;
using FilterLib.Filters.Noise;
using FilterLib.Filters.Other;
using FilterLib.Filters.Sharpen;
using FilterLib.Filters.Transform;
using FilterLib.Util;
using System.Diagnostics;

List<IFilter> filters =
[
    // Adjustments
    new AutoLevelsFilter(),
    new BrightnessFilter(123),
    new ColorHSLFilter(100, 40, -40),
    new ColorRGBFilter(50, -50, 60),
    new ContrastFilter(70),
    new GammaFilter(3f),
    new LevelsFilter(40, 120),
    new ShadowsHighlightsFilter(20, 40),
    // Artistic
    new AdaptiveTresholdFilter(10),
    new OilPaintFilter(10, 127),
    new RandomJitterFilter(20, 0),
    // Blur
    new BlurFilter(),
    new BoxBlurFilter(10, 20),
    new GaussianBlurFilter(20),
    new MotionBlurFilter(20, 32),
    new SpinBlurFilter(Size.Relative(.5f), Size.Relative(.5f), 30, 10),
    new ZoomBlurFilter(Size.Relative(.5f), Size.Relative(.5f), 70, 10),
    // Border
    new FadeBorderFilter(Size.Absolute(20), new(0, 0, 0)),
    new JitterBorderFilter(Size.Relative(.2f), new(0, 0, 0), 0),
    new PatternBorderFilter(Size.Relative(.2f), Size.Relative(.2f), new(10, 10), BorderPosition.Outside, AntiAliasQuality.Medium),
    new SimpleBorderFilter(Size.Relative(.2f), Size.Relative(.2f), new(0, 0, 0), BorderPosition.Outside, AntiAliasQuality.Medium),
    new VignetteFilter(Size.Relative(1f), Size.Relative(.5f), new(0, 0, 0)),
    // Color
    new GradientMapFilter(new(new(0, 0, 255), new(0, 255, 255), new(0, 255, 0))),
    new GrayscaleFilter(33, 34, 33),
    new InvertFilter(),
    new OctreeQuantizerFilter(32),
    new OrtonFilter(50, 10),
    new PosterizeFilter(4),
    new SepiaFilter(),
    new SolarizeFilter(),
    new TresholdFilter(127),
    new VintageFilter(50),
    // Dither
    new AtkinsonDitherFilter(4),
    new BayerDitherFilter(4, 8),
    new BurkesDitherFilter(4),
    new ClusterDotDitherFilter(),
    new FanDitherFilter(4),
    new FilterLiteDitherFilter(4),
    new FloydSteinbergDitherFilter(4),
    new JarvisJudiceNinkeDitherFilter(4),
    new RandomDitherFilter(4, 0),
    new ShiauFanDitherFilter(4),
    new SierraDitherFilter(4),
    new StuckiDitherFilter(4),
    // Edges
    new EdgeDetectionFilter(),
    new EmbossFilter(),
    new PrewittFilter(),
    new ScharrFilter(),
    new SobelFilter(),
    // Generate
    new LinearGradientFilter(Size.Relative(0), Size.Relative(0), Size.Relative(1), Size.Relative(1)),
    new MarbleFilter(10, 10, 5, 5, 0),
    new RadialGradientFilter(Size.Relative(.5f), Size.Relative(.5f), Size.Relative(.1f), Size.Relative(.9f)),
    new TurbulenceFilter(5, 0),
    new WoodRingsFilter(5, 5, 5, 0),
    // Mosaic
    new CrystallizeFilter(20, 50, 0),
    new LegoFilter(32, AntiAliasQuality.Medium),
    new PixelateFilter(32, PixelateFilter.PixelateMode.Average),
    // Noise
    new AddNoiseFilter(500, 127, AddNoiseFilter.NoiseType.Color, 0),
    new MedianFilter(50, 2),
    // Other
    new ConvertToPolarFilter(30),
    new ConvolutionFilter(new ConvolutionMatrix(new int[,] { { 1, 1, 1 }, { 1, 1, 1 }, { 1, 1, 1 } }, 9, 30)),
    new EquirectangularToStereographicFilter(90, 30, InterpolationMode.Bilinear),
    new WavesFilter(Size.Relative(.5f), Size.Relative(.3f), Direction.Horizontal),
    // Sharpen
    new MeanRemovalFilter(),
    new SharpenFilter(),
    // Transform
    new BoxDownscaleFilter(Size.Relative(.5f), Size.Relative(.5f)),
    new CropFilter(Size.Relative(.1f), Size.Relative(.1f), Size.Relative(.9f), Size.Relative(.9f)),
    new FlipHorizontalFilter(),
    new FlipVerticalFilter(),
    new ResizeFilter(Size.Relative(1.1f), Size.Relative(1.1f), InterpolationMode.Bilinear),
    new Rotate180Filter(),
    new RotateFilter(10, RotateFilter.CropMode.Fit, InterpolationMode.Bilinear),
    new RotateLeftFilter(),
    new RotateRightFilter(),
    new SkewFilter(45),
];


const int WIDTH = 4000;
const int HEIGHT = 3000;
const int REP_MIN = 2;
const int REP_MAX = 10;
const int REP_TOTAL_TIME_MINIMUM_MS = 2000;

Image img = new(WIDTH, HEIGHT);
Random random = new();
for (int x = 0; x < WIDTH; x++)
    for (int y = 0; y < HEIGHT; y++)
        for (int c = 0; c < 3; ++c)
            img[x, y, c] = (byte)random.Next(256);

foreach (var f in filters)
{
    long total = 0;
    int n = 0;
    while (n < REP_MAX && (n < REP_MIN || total < REP_TOTAL_TIME_MINIMUM_MS))
    {
        Stopwatch sw = Stopwatch.StartNew();
        f.Apply(img);
        sw.Stop();
        total += sw.ElapsedMilliseconds;
        ++n;
    }
    Console.WriteLine($"{f}\t{total / n}");
}

foreach(var f in ReflectiveApi.GetFilterTypes())
    if (!filters.Any(f0 => f0.GetType() == f))
        Console.WriteLine($"Warning: no test for {f}");
