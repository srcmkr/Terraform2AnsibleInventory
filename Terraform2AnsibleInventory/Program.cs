using CommandLine;
using System;
using System.Collections.Generic;

namespace Terraform2AnsibleInventory
{
    class Program
    {
        static void Main(string[] args)
        {
            Parser.Default.ParseArguments<Options>(args)
                .WithParsed(RunOptions)
                .WithNotParsed(HandleParseError);
        }

        static void RunOptions(Options opts)
        {
            var t2ai = new T2AI();

            if (t2ai.PreflightCheck(opts.InputFile))
                if (t2ai.LoadJson())
                    if(t2ai.SaveToFile(opts.OutputFile))
                        if(opts.Verbose)
                        {
                            Console.WriteLine(t2ai.sb);
                        } else
                        {
                            Console.WriteLine("Terraform to Ansible Inventory by label converter");
                            Console.WriteLine($"https://github.com/srcmkr/Terraform2AnsibleInventory\r\n");
                            Console.WriteLine($"'{opts.InputFile}' converted successfully and saved to '{opts.OutputFile}'");
                        }
                            
        }
        static void HandleParseError(IEnumerable<Error> errs)
        {
            Console.WriteLine("Usage: t2ai --in /path/to/terraform.tfstate --out /path/to/ansible_hosts [--verbose]");
        }

        class Options
        {
            [Option('i', "in", Required = true, HelpText = "Input file to be processed.")]
            public string InputFile { get; set; }

            [Option('o', "out", Required = true, HelpText = "Output file to be processed.")]
            public string OutputFile { get; set; }

            [Option('v', "verbose", Required = false, Default = false, HelpText = "Verbose all messages to standard output.")]
            public bool Verbose { get; set; }
        }
    }
}
