using System;
using System.IO;
using JMeter.Toolkit.Engine.Extensions;
using JMeter.Toolkit.Services;
using JMeter.Toolkit.Services.Spec;
using NDesk.Options;

namespace JMeter.Toolkit.CLI.Commands
{
    public class FileCommand : CommandBase, ICommand, ICommandFactory
    {
        private string _file = string.Empty;
        private readonly JMeterLogService _service = new JMeterLogService();
        
        public FileCommand()
        {
            _optionSet = new OptionSet
                        {
                            { "f|file=", v => _file = v }
                       };
        }

        public FileCommand(string[] arguments) : this()
        {
            _optionSet.Parse(arguments);
        }

        public FileCommand(string file) : this()
        {
            _file = file;
        }

        public void Execute()
        {
            var fileName = Path.GetFileNameWithoutExtension(_file);
            var path = Path.GetDirectoryName(_file);
            if (path != null)
            {
                var output = Path.Combine(path, fileName + "_" + DateTime.Now.ToString("ddMMyyyy") + ".jpeg");
                using (var fs = new FileStream(_file, FileMode.Open, FileAccess.Read))
                {
                    var request = new JMeterLogsRequest {Logs = fs.ReadContent()};

                    var dataResultsResponse = _service.ProcessLogs(request);
                    var chartResponse =
                        _service.GenerateCharts(new JMeterDataResultsRequest { DataResults = dataResultsResponse.DataResults });
                    using (var outputStream = new FileStream(output, FileMode.Create, FileAccess.ReadWrite))
                    {
                        outputStream.Write(chartResponse.Charts[0].Data, 0, chartResponse.Charts[0].Data.Length);
                        Console.WriteLine("{0} Done", output);
                    }
                }
            }
        }

        public string CommandName
        {
            get { return "ProcessFile"; }
        }

        public string Description
        {
            get { return "Process JMeter Log file"; }
        }

        public ICommand CreateCommand(string[] arguments)
        {
            return new FileCommand(arguments);
        }
    }
}
