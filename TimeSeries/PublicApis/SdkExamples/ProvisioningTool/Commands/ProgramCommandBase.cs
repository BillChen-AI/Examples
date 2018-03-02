using System;
using System.Reflection;
using log4net;

namespace ProvisioningTool.Commands
{
    public abstract class ProgramCommandBase : IProgramCommand
    {
        protected static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        protected Context Context { get; set; }

        public string Id { get; set; }
        public string HelpText { get; set; }

        protected ProgramCommandBase(string commandId, string helpText)
        {
            if(string.IsNullOrWhiteSpace(commandId))
                throw new ArgumentNullException(nameof(commandId));

            Id = commandId;
            HelpText = helpText;
        }

        public abstract void Run();
    }
}
