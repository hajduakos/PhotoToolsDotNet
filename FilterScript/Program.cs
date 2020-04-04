using FilterScript.Model;
using System;
using System.Drawing;
using System.IO;

namespace FilterScript
{
    static class Program
    {
        static void Main(string[] args)
        {
            string scriptPath = ParseArg(args, "s");
            string inputPath = ParseArg(args, "i");
            string outputPath = ParseArg(args, "o");
            Batch batch = Parser.Parse(File.ReadAllLines(scriptPath));
            using Bitmap input = new Bitmap(inputPath);
            batch.InputTask.Input = input;
            using Bitmap output = batch.Execute();
            output.Save(outputPath);
        }

        static string ParseArg(string[] args, string flag)
        {
            flag = "-" + flag;
            for(int i = 0; i < args.Length; ++i)
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
