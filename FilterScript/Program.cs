using FilterLib;
using FilterLib.IO;
using FilterScript.Model;
using System;
using System.IO;

namespace FilterScript
{
    static class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 1 && args[0] == "--doc")
            {
                using StreamWriter sw = new(Console.OpenStandardOutput());
                new DocGen(sw).Write();
                return;
            }
            string scriptPath = ParseArg(args, "s");
            string inputPath = ParseArg(args, "i");
            string outputPath = ParseArg(args, "o");
            Script batch = Parser.Parse(File.ReadAllLines(scriptPath));
            Image input = FilterLib.Util.BitmapAdapter.FromBitmapPath(inputPath);
            new BitmapCodec().WriteFile(batch.Execute(input), outputPath);
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
