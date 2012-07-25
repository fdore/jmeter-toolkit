using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using JMeter.Toolkit.CLI.Commands;

namespace JMeter.Toolkit.CLI
{
    public class Program
    {
        /// <summary>
        /// Mains the specified args.
        /// </summary>
        /// <param name="args">The args.</param>
        static void Main(string[] args)
        {
            try
            {
                var parser = new CommandParser(GetAvailableCommands());
                var command = parser.ParseCommand(args);
                command.Execute();
            }
            catch (Exception e)
            {
                DisplayHelp();
                Console.WriteLine(e);
            }
        }

        /// <summary>
        /// Load executing assembly, and builds list of available commands
        /// </summary>
        /// <returns></returns>
        static IEnumerable<ICommandFactory> GetAvailableCommands()
        {
            var query = Assembly.GetExecutingAssembly().GetExportedTypes().Where(t => t.GetInterface("ICommand") != null && t.GetInterface("ICommandFactory") != null);
            return query.Select(type => (ICommandFactory)Activator.CreateInstance(type));
        }

        /// <summary>
        /// Display program usage
        /// </summary>
        private static void DisplayHelp()
        {
            Console.WriteLine("Usage: CommandName Arguments");
            Console.WriteLine("Commands:");
            foreach (var cmd in GetAvailableCommands())
            {
                Console.WriteLine("  {0}: {1}", cmd.CommandName, cmd.Description);
                Console.WriteLine("{0}", cmd.Options);
            }
        }
    }

}
