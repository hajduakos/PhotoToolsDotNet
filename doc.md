# FilterScript

## Filters

### Adjustments

**AutoLevels**

**Brightness**
- Brightness: Int32[-255;255]

**ColorHSL**
- Hue: Int32[-180;180]
- Saturation: Int32[-100;100]
- Lightness: Int32[-100;100]

**ColorRGB**
- Red: Int32[-255;255]
- Green: Int32[-255;255]
- Blue: Int32[-255;255]

**Contrast**
- Contrast: Int32[-100;100]

**Gamma**
- Gamma: Single[0.001;...]

**Levels**
- Dark: Int32[0;255]
- Light: Int32[0;255]

**ShadowsHighlights**
- Brighten: Int32[0;100]
- Darken: Int32[0;100]

### Artistic

**AdaptiveTreshold**
- SquareSize: Int32[1;...]

**OilPaint**
- Radius: Int32[1;...]

**RandomJitter**
- Radius: Int32[1;...]
- Seed: Int32

### Blur

**Blur**

**BoxBlur**
- RadiusX: Int32[0;...]
- RadiusY: Int32[0;...]

**GaussianBlur**
- Radius: Int32[0;...]

**MotionBlur**
- Length: Int32[0;...]
- Angle: Single[0;360]

### Border

**FadeBorder**
- Width: Size
- Color: RGB

**JitterBorder**
- Width: Size
- Color: RGB
- Seed: Int32

**PatternBorder**
- Width: Size
- Radius: Size
- Pattern: Bitmap
- Position: BorderPosition

**SimpleBorder**
- Width: Size
- Radius: Size
- Color: RGB
- Position: BorderPosition

**Vignette**
- Radius: Size
- ClearRadius: Size

### Color

**GradientMap**
- GradientMap: Gradient

**Grayscale**
- Red: Int32[0;100]
- Green: Int32[0;100]
- Blue: Int32[0;100]

**Invert**

**Orton**
- Strength: Int32[0;100]
- Radius: Int32[0;...]

**Posterize**
- Levels: Int32[2;256]

**Sepia**

**Treshold**
- Treshold: Int32[0;255]

**Vintage**
- Strength: Int32[0;100]

### Dither

**AtkinsonDither**
- Levels: Int32[2;256]

**BayerDither**
- Size: Int32[1;...]
- Levels: Int32[2;256]

**BurkesDither**
- Levels: Int32[2;256]

**FanDither**
- Levels: Int32[2;256]

**FloydSteinbergDither**
- Levels: Int32[2;256]

**JarvisJudiceNinkeDither**
- Levels: Int32[2;256]

**RandomDither**
- Levels: Int32[2;256]
- Seed: Int32

**ShiauFanDither**
- Levels: Int32[2;256]

**SierraDither**
- Levels: Int32[2;256]

**StuckiDither**
- Levels: Int32[2;256]

### Generate

**Marble**
- HorizontalLines: Int32[0;...]
- VerticalLines: Int32[0;...]
- Twist: Single[0;...]
- Iterations: Int32[1;...]
- Seed: Int32

**Turbulence**
- Iterations: Int32[1;...]
- Seed: Int32

**WoodRings**
- Twist: Single[0;...]
- Rings: Int32[0;...]
- Iterations: Int32[1;...]
- Seed: Int32

### Mosaic

**Crystallize**
- Size: Int32[1;...]
- Averaging: Int32[0;100]
- Seed: Int32

**Lego**
- Size: Int32[8;...]

**Pixelate**
- Size: Int32[1;...]

### Noise

**AddNoise**
- Intensity: Int32[0;1000]
- Strength: Int32[0;255]
- Type: NoiseType
- Seed: Int32

**Median**
- Strength: Int32[0;100]

### Other

**ConvertToPolar**
- Phase: Single[0;360]

**Convolution**
- Matrix: Conv3x3

**EquirectangularToStereographic**
- AOV: Single[0;179.999]
- Spin: Single[0;360]

**Waves**
- Wavelength: Size
- Amplitude: Size
- Direction: WaveDirection

### Sharpen

**EdgeDetection**

**Emboss**

**Prewitt**

**Sobel**

**MeanRemoval**

**Sharpen**

### Transform

**Crop**
- X: Size
- Y: Size
- Width: Size
- Height: Size

**FlipHorizontal**

**FlipVertical**

**Resize**
- Width: Size
- Height: Size
- Interpolation: InterpolationMode

**Rotate180**

**Rotate**
- Angle: Single[0;360]
- Crop: Boolean

**RotateLeft**

**RotateRight**

