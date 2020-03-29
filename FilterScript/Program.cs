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
            if (args[0] == "--doc")
            {
                using StreamWriter sw = new StreamWriter(Console.OpenStandardOutput());
                new DocGen(sw).Write();
                return;
            }
            Batch batch = Parser.Parse(File.ReadAllLines(args[0]));
            using Bitmap input = new Bitmap(args[1]);
            batch.InputTask.Input = input;
            using Bitmap output = batch.Execute();
            output.Save(args[2]);
        }
    }
}
