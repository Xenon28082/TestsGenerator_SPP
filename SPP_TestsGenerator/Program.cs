using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace SPP_TestsGenerator
{
    
    
    
    class Program
    {
        
        private static Generator.Generator _generator = new Generator.Generator();

        private static async Task<PipeFrame[]> Read(PipeFrame[] args)
        {
            return await Task<PipeFrame[]>.Factory.StartNew(() => args.Select(x => new PipeFrame(x.path, File.ReadAllText(x.path))).ToArray());
        }

        private static async Task<PipeFrame[]> Generate(PipeFrame[] args)
        {
            return await Task<PipeFrame[]>.Factory.StartNew(() => args.Select(x => new PipeFrame(x.path, _generator.Generate(x.code).Result)).ToArray());
        }

        private static void Write(PipeFrame[] args)
        {
            foreach (PipeFrame arg in args)
            {
                File.WriteAllTextAsync(Path.GetFileNameWithoutExtension(arg.path) + ".test" + Path.GetExtension(arg.path), arg.code);
            }
        }

        private static IEnumerable<IEnumerable<PipeFrame>> CreateFrames(string[] pathes, int max)
        {
            int maxFiles = max;
            var paths = new List<string>();

            for (int i = 1; i < pathes.Length; i++)
            {
                paths.Add(pathes[i]);
            }

            var inputs = new List<List<PipeFrame>>();

            for (var i = 0; i < (float)paths.Count / maxFiles; i++)
            {
                yield return paths.Skip(i * maxFiles).Take(maxFiles).Select(x => new PipeFrame(x));
            }
        }
        static void Main(string[] args)
        {
            string[] pathes = {"", "C:\\Users\\savva\\RiderProjects\\SPP\\TestDLL\\TestDLL\\Program.cs", "C:\\Users\\savva\\RiderProjects\\SPP\\TracerSPP\\ConsoleApp1\\TracerResources\\Tracer.cs"};
            IEnumerable<IEnumerable<PipeFrame>> inputs = CreateFrames(pathes, 2);
            List<ActionBlock<PipeFrame[]>> waits = new List<ActionBlock<PipeFrame[]>>();

            foreach (IEnumerable<PipeFrame> input in inputs)
            {

                var readedFiles = new TransformBlock<PipeFrame[], PipeFrame[]>(Read);
                var generatedTests = new TransformBlock<PipeFrame[], PipeFrame[]>(Generate);
                var savedTests = new ActionBlock<PipeFrame[]>(Write);

                var linkOptions = new DataflowLinkOptions { PropagateCompletion = true };

                readedFiles.LinkTo(generatedTests, linkOptions);
                generatedTests.LinkTo(savedTests, linkOptions);

                readedFiles.Post(input.ToArray());
                readedFiles.Complete();

                waits.Add(savedTests);
            }

            foreach (ActionBlock<PipeFrame[]> action in waits)
            {
                action.Completion.Wait();
            }
            Console.Write("Gone");
        }
    }
}