namespace JMeter.Toolkit.CLI.Commands
{
    public interface ICommandFactory
    {
        string CommandName { get; }
        string Description { get; }
        string Options { get; }

        ICommand CreateCommand(string[] arguments);
    }
}
