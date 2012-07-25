using System.IO;
using System.Text;
using NDesk.Options;

namespace JMeter.Toolkit.CLI.Commands
{
    public abstract class CommandBase
    {
        protected OptionSet _optionSet;

        public string Options
        {
            get
            {
                var sb = new StringBuilder();
                var sw = new StringWriter(sb);
                _optionSet.WriteOptionDescriptions(sw);
                return sb.ToString();
            }
        }


    }
}
