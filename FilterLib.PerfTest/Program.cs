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

List<IFilter> filters = new();
filters.Add(new AutoLevelsFilter());
filters.Add(new BrightnessFilter(123));
filters.Add(new ColorHSLFilter(100, 40, -40));
filters.Add(new ColorRGBFilter(50, -50, 60));
filters.Add(new ContrastFilter(70));
filters.Add(new GammaFilter(3f));
filters.Add(new LevelsFilter(40, 120));
filters.Add(new ShadowsHighlightsFilter(20, 40));

filters.Add(new AdaptiveTresholdFilter(10));
filters.Add(new OilPaintFilter(10, 127));
filters.Add(new RandomJitterFilter(20, 0));

filters.Add(new BlurFilter());
filters.Add(new BoxBlurFilter(10, 20));
filters.Add(new GaussianBlurFilter(20));
filters.Add(new MotionBlurFilter(20, 32));
filters.Add(new SpinBlurFilter(Size.Relative(.5f), Size.Relative(.5f), 30, 10));
filters.Add(new ZoomBlurFilter(Size.Relative(.5f), Size.Relative(.5f), 70, 10));

filters.Add(new FadeBorderFilter(Size.Absolute(20), new(0, 0, 0)));
filters.Add(new JitterBorderFilter(Size.Relative(.2f), new(0, 0, 0), 0));
filters.Add(new PatternBorderFilter(Size.Relative(.2f), Size.Relative(.2f), new(10, 10), BorderPosition.Outside, AntiAliasQuality.Medium));
filters.Add(new SimpleBorderFilter(Size.Relative(.2f), Size.Relative(.2f), new(0, 0, 0), BorderPosition.Outside, AntiAliasQuality.Medium));
filters.Add(new VignetteFilter(Size.Relative(1f), Size.Relative(.5f), new(0, 0, 0)));

filters.Add(new GradientMapFilter(new(new(0, 0, 255), new(0, 255, 255), new(0, 255, 0))));
filters.Add(new GrayscaleFilter(33, 34, 33));
filters.Add(new InvertFilter());
filters.Add(new OctreeQuantizerFilter(32));
filters.Add(new OrtonFilter(50, 10));
filters.Add(new PosterizeFilter(4));
filters.Add(new SepiaFilter());
filters.Add(new SolarizeFilter());
filters.Add(new TresholdFilter(127));
filters.Add(new VintageFilter(50));

filters.Add(new AtkinsonDitherFilter(4));
filters.Add(new BayerDitherFilter(4, 8));
filters.Add(new BurkesDitherFilter(4));
filters.Add(new FanDitherFilter(4));
filters.Add(new FloydSteinbergDitherFilter(4));
filters.Add(new JarvisJudiceNinkeDitherFilter(4));
filters.Add(new RandomDitherFilter(4, 0));
filters.Add(new ShiauFanDitherFilter(4));
filters.Add(new SierraDitherFilter(4));
filters.Add(new StuckiDitherFilter(4));

filters.Add(new EdgeDetectionFilter());
filters.Add(new EmbossFilter());
filters.Add(new PrewittFilter());
filters.Add(new SobelFilter());

filters.Add(new LinearGradientFilter(Size.Relative(0), Size.Relative(0), Size.Relative(1), Size.Relative(1)));
filters.Add(new MarbleFilter(10, 10, 5, 5, 0));
filters.Add(new RadialGradientFilter(Size.Relative(.5f), Size.Relative(.5f), Size.Relative(.1f), Size.Relative(.9f)));
filters.Add(new TurbulenceFilter(5, 0));
filters.Add(new WoodRingsFilter(5, 5, 5, 0));

filters.Add(new CrystallizeFilter(20, 50, 0));
filters.Add(new LegoFilter(32, AntiAliasQuality.Medium));
filters.Add(new PixelateFilter(32, PixelateFilter.PixelateMode.Average));

filters.Add(new AddNoiseFilter(500, 127, AddNoiseFilter.NoiseType.Color, 0));
filters.Add(new MedianFilter(50, 2));

filters.Add(new ConvertToPolarFilter(30));
filters.Add(new ConvolutionFilter(new Conv3x3(1, 1, 1, 1, 1, 1, 1, 1, 1, 9, 30)));
filters.Add(new EquirectangularToStereographicFilter(90, 30, InterpolationMode.Bilinear));
filters.Add(new WavesFilter(Size.Relative(.5f), Size.Relative(.3f), WavesFilter.WaveDirection.Horizontal));

filters.Add(new MeanRemovalFilter());
filters.Add(new SharpenFilter());

filters.Add(new BoxDownscaleFilter(Size.Relative(.5f), Size.Relative(.5f)));
filters.Add(new CropFilter(Size.Relative(.1f), Size.Relative(.1f), Size.Relative(.9f), Size.Relative(.9f)));
filters.Add(new FlipHorizontalFilter());
filters.Add(new FlipVerticalFilter());
filters.Add(new ResizeFilter(Size.Relative(1.1f), Size.Relative(1.1f), InterpolationMode.Bilinear));
filters.Add(new Rotate180Filter());
filters.Add(new RotateFilter(10, RotateFilter.CropMode.Fit, InterpolationMode.Bilinear));
filters.Add(new RotateLeftFilter());
filters.Add(new RotateRightFilter());

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
