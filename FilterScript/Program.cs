using FilterScript.Model;
using System.Drawing;
using System.IO;

namespace FilterScript
{
    static class Program
    {
        static void Main(string[] args)
        {
            Batch batch = Parser.Parse(File.ReadAllLines(args[0]));
            using Bitmap input = new Bitmap(args[1]);
            batch.InputTask.Input = input;
            using Bitmap output = batch.Execute();
            output.Save(args[2]);
        }
    }
}
