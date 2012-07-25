using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using JMeter.Toolkit.Engine.Extensions;
using JMeter.Toolkit.Services;
using JMeter.Toolkit.Services.Spec;
using NDesk.Options;

namespace JMeter.Toolkit.CLI.Commands
{
    public class DirectoryCommand : CommandBase, ICommand, ICommandFactory
    {
        private string _directory;
        private string _outputDirectory;
        private bool _merge;
        private readonly JMeterLogService _service = new JMeterLogService();
        
        public DirectoryCommand()
        {
            _optionSet = new OptionSet
                        {
                            { "dir=",      v => _directory = v },
                            { "outputDir=",      v => _outputDirectory = v },
                            { "merge=",      v => _merge = (v == "yes") ? _merge = true : _merge = false},
                       };
        }

        private DirectoryCommand(string[] arguments) : this()
        {
            _optionSet.Parse(arguments);
        }

        public void Execute()
        {
            string[] filePaths = Directory.GetFiles(_directory, "*.jtl", SearchOption.AllDirectories);
            if (filePaths.Length > 0)
            {
                Console.WriteLine("{0} file{1} to process...", filePaths.Length, filePaths.Length > 1 ? "s" : "");
                if (_merge)
                {
                    Console.WriteLine("Creating Trend Charts...");
                    var dataResults = new List<RequestDataResults>();
                    // Process files and merge results
                    foreach (var file in filePaths)
                    {
                        using (var fs = new FileStream(file, FileMode.Open, FileAccess.Read))
                        {
                            var dataResultsResponse = _service.ProcessLogs(new JMeterLogsRequest { Logs = fs.ReadContent() });
                            dataResults.AddRange(dataResultsResponse.DataResults);
                        }
                    }

                    // Generate chart
                    var chartResponse = _service.GenerateCharts(new JMeterDataResultsRequest { DataResults = dataResults.ToArray() });

                    // Save image
                    chartResponse.Charts.AsParallel().ForAll(chart =>
                    {
                        var fileName = string.Format(
                            "{0}_{1}.jpeg", DateTime.Now.ToString("ddMMyyyy"), chart.Name.Replace('/', '_'));
                        string output = Path.Combine(_directory, fileName);
                        if (_outputDirectory != null)
                        {
                            if (!Directory.Exists(_outputDirectory))
                                Directory.CreateDirectory(_outputDirectory);
                            output = Path.Combine(_outputDirectory, fileName);
                        }

                        using (
                            var outputStream = new FileStream(output,
                                                              FileMode.
                                                                  Create,
                                                              FileAccess.
                                                                  ReadWrite)
                            )
                        {
                            Console.WriteLine("Writing {0} ... ({1}KB)",
                                              fileName,
                                              chart.Data.Length / 1000);
                            outputStream.Write(chart.Data, 0,
                                               chart.Data.Length);
                            Console.WriteLine("{0} Done", output);
                        }
                    });

                }
                else
                {
                    Console.WriteLine("Creating Comparison Charts...");
                    foreach (var file in filePaths)
                    {
                        var cmd = new FileCommand(file);
                        cmd.Execute();
                    }
                }
            }
            else
            {
                Console.WriteLine("No file to process.");
            }
        }

        public string CommandName
        {
            get { return "ProcessDirectory"; }
        }

        public string Description
        {
            get { return "Process JMeter Log files contained in a directory. This command is recursive."; }
        }


        public ICommand CreateCommand(string[] arguments)
        {
            return new DirectoryCommand(arguments);
        }
    }
}
