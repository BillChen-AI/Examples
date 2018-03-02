using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using log4net;

namespace ProvisioningTool.Commands
{
    public class ProgramRunner
    {
        private static readonly string AllCommandKey = MakeCommandMapKey("All");

        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private readonly IDictionary<string, IProgramCommand> _commandMap =
            new Dictionary<string, IProgramCommand>();

        public void AddCommand(IProgramCommand command)
        {
            if (command == null)
                throw new ArgumentNullException(nameof(command));

            if (string.IsNullOrWhiteSpace(command.Id))
                throw new ArgumentException("Missing command id.");

            ThrowIfCommandAlreadExists(command);
            _commandMap.Add(MakeCommandMapKey(command.Id), command);
        }

        private void ThrowIfCommandAlreadExists(IProgramCommand command)
        {
            var commandKey = MakeCommandMapKey(command.Id);

            if (_commandMap.ContainsKey(commandKey))
            {
                throw new ArgumentException(nameof(command));
            }
        }

        private static string MakeCommandMapKey(string arg)
        {
            return arg.ToUpper();
        }

        public void Run(string[] args)
        {
            if (HasNoValidCommand(args))
            {
                DisplayHelpForAllCommands();
                return;
            }

            if (args.Any(arg => MakeCommandMapKey(arg) == AllCommandKey))
            {
                RunAll();
            }
            else
            {
                RunSpecifiedCommands(args);
            }

            Log.Info("Done!");
        }

        private bool HasNoValidCommand(string[] args)
        {
            var allValidCommandKeys = new List<string>(_commandMap.Keys) {AllCommandKey};

            return !args.Any(arg => allValidCommandKeys.Contains(MakeCommandMapKey(arg)));
        }

        private void DisplayHelpForAllCommands()
        {
            Console.WriteLine("Please supply one or more commands to run:");
            Console.WriteLine("----------------------------------------------------------");
            Console.WriteLine("  All:");
            Console.WriteLine("    Run all commands.");

            foreach (var idCommandPair in _commandMap)
            {
                var command = idCommandPair.Value;
                Console.WriteLine($"  {command.Id}:");
                Console.WriteLine($"    {command.HelpText}");
            }

            Console.WriteLine("----------------------------------------------------------");
        }

        private void RunAll()
        {
            foreach (var idCommandPair in _commandMap)
            {
                idCommandPair.Value.Run();
            }
        }

        private void RunSpecifiedCommands(string[] args)
        {
            foreach (var arg in args)
            {
                var commandKey = MakeCommandMapKey(arg);
                if (_commandMap.ContainsKey(commandKey))
                {
                    Log.Info($"Running command {arg}...");
                    _commandMap[commandKey].Run();
                    Log.Info($"Completed running command {arg}.");
                }
            }
        }
    }
}
