using System.Collections.Generic;
using System.Linq;

namespace JMeter.Toolkit.CLI.Commands
{
    public class CommandParser
    {
        private readonly IEnumerable<ICommandFactory> _availableCommands;

        public CommandParser(IEnumerable<ICommandFactory> availableCommands)
        {
            _availableCommands = availableCommands;
        }

        internal ICommand ParseCommand(string[] args)
        {
            var requestedCommand = args[0];
            var command = FindRequestedCommand(requestedCommand);
            if (command == null)
                return new NotFoundCommand {Name = requestedCommand};
            return command.CreateCommand(args);
        }

        ICommandFactory FindRequestedCommand(string commandName)
        {
            return _availableCommands.FirstOrDefault(c => c.CommandName == commandName);
        }
    }
}
