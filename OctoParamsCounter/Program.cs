using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using CommandLine;

namespace OctoParamsCounter
{
    class Options
    {
        [Option('i', "input", Required = true, HelpText = "Input files to be processed. e.g. *.json or foo.json")]
        public IEnumerable<string> InputFiles { get; set; }

        [Option('d', "directory", Required = false, HelpText = "Directory to search.", Default = ".")]
        public string Directory { get; set; }

        [Option('p', "pattern", Required = false, HelpText = "Regex for Octopus parameters. Default is \"(\\#\\{|OctopusParameters\\[)(.*)\"", Default = "(\\#\\{|OctopusParameters\\[)(.*)")]
        public string OctoParamPattern { get; set; }

        [Option('r', "recursive", Required = false, HelpText = "Recursive search", Default = true)]
        public bool Recursive { get; set; }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Parser.Default.ParseArguments<Options>(args)
                .WithParsed(RunOptionsAndReturnExitCode);
        }

        private static void RunOptionsAndReturnExitCode(Options opts)
        {
            try
            {
                Regex search = new Regex(opts.OctoParamPattern);
                foreach (string inputFilePattern in opts.InputFiles)
                {
                    foreach (string file in Directory.GetFiles(opts.Directory, inputFilePattern, opts.Recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly))
                    {
                        string[] lines = File.ReadAllLines(file);
                        for (int i = 0; i < lines.Length; i++)
                        {
                            string line = lines[i];
                            if (search.IsMatch(line))
                            {
                                Console.WriteLine($"{file}({i}) : {line}");
                            }
                        }
                    }
                }
            }
            catch (Exception excpt)
            {
                Console.WriteLine(excpt.Message);
            }
        }
    }
}
