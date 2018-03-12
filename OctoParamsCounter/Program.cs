﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using CommandLine;

namespace OctoParamsCounter
{
    class Options
    {
        private const string DefaultPattern = @"(\#\{|OctopusParameters\[)([^\/\]\}]+)";

        [Option('i', "input", Required = true, HelpText = "Input files to be processed. e.g. *.json or foo.json")]
        public IEnumerable<string> InputFiles { get; set; }

        [Option('d', "directory", Required = false, HelpText = "Directory to search.", Default = ".")]
        public string Directory { get; set; }

        [Option('p', "pattern", Required = false, HelpText = "Regex for Octopus parameters. Default is " + DefaultPattern, Default = DefaultPattern)]
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
                Regex search = new Regex(opts.OctoParamPattern, RegexOptions.Compiled);
                foreach (string inputFilePattern in opts.InputFiles)
                {
                    foreach (string file in Directory.GetFiles(opts.Directory, inputFilePattern, opts.Recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly))
                    {
                        string[] lines = File.ReadAllLines(file);
                        for (int i = 0; i < lines.Length; i++)
                        {
                            string line = lines[i];
                            foreach (Match match in search.Matches(line))
                            {
                                string octoParam = TrimOctoParam(match.Groups[2].Value);
                                Console.WriteLine($"{file}({i}) : {octoParam}");
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

        private static string TrimOctoParam(string octoParam)
        {
            return octoParam.Replace("'", "").Replace(@"\", "").Replace("\"", "") // remove quotes
                .Split("|").First().Trim() // remove filters.
                .Split(" ").Last().Trim(); // remove conditionals
        }
    }
}
