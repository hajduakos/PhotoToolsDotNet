using FilterScript.Model;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace FilterScript
{
    static class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 1 && args[0] == "--doc")
            {
                using StreamWriter sw = new StreamWriter(Console.OpenStandardOutput());
                new DocGen(sw).Write();
                return;
            }
            string scriptPath = ParseArg(args, "s");
            string inputPath = ParseArg(args, "i");
            string outputPath = ParseArg(args, "o");
            Batch batch = Parser.Parse(File.ReadAllLines(scriptPath));
            using Bitmap input = new Bitmap(inputPath);
            batch.InputTask.Input = input;
            using Bitmap output = batch.Execute();
            output.Save(outputPath, ImageFormat.Bmp);
            switch(new FileInfo(outputPath).Extension.ToLower())
            {
                case ".bmp":
                    output.Save(outputPath, ImageFormat.Bmp);
                    break;
                case ".gif":
                    output.Save(outputPath, ImageFormat.Gif);
                    break;
                case ".png":
                    output.Save(outputPath, ImageFormat.Png);
                    break;
                case ".tiff":
                    output.Save(outputPath, ImageFormat.Tiff);
                    break;
                case ".jpg":
                case ".jpeg":
                    EncoderParameters eps = new EncoderParameters(1) { Param = new EncoderParameter[] { new EncoderParameter(Encoder.Quality, 97L) } };
                    if (GetEncoder(ImageFormat.Jpeg) == null) Console.WriteLine("NULL");
                    output.Save(outputPath, GetEncoder(ImageFormat.Jpeg), eps);
                    break;
                default:
                    Console.WriteLine("Unsupported output extension.");
                    break;
            }
        }

        private static ImageCodecInfo GetEncoder(ImageFormat format)
        {
            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageDecoders();
            foreach (ImageCodecInfo codec in codecs)
                if (codec.FormatID == format.Guid)
                    return codec;
            return null;
        }

        static string ParseArg(string[] args, string flag)
        {
            flag = "-" + flag;
            for (int i = 0; i < args.Length; ++i)
            {
                if (args[i] == flag)
                {
                    if (i == args.Length - 1) throw new ArgumentException($"Value not found for argument {flag}.");
                    return args[i + 1];
                }
            }

            throw new ArgumentException($"Argument {flag} not found.");
        }
    }
}
