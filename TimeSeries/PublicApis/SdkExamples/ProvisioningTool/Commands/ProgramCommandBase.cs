using System;

namespace ProvisioningTool.Commands
{
    public abstract class ProgramCommandBase : IProgramCommand
    {
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
