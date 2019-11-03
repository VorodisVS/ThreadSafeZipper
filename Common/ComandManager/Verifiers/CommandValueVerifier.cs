using System.Collections.Generic;
using System.Linq;

namespace Common.ComandManager.Verifiers
{
    public class CommandValueVerifier : IVerifier
    {
        private readonly IEnumerable<string> _aviabledCommands;

        public CommandValueVerifier(IEnumerable<string> aviableCommands)
        {
            _aviabledCommands = aviableCommands;
        }

        public bool TryVerify(string[] args, out string errorMessage)
        {
            errorMessage = string.Empty;
            if (_aviabledCommands.Contains(args[0]))
                return true;

            errorMessage = $"Command {args[0]} not specified";
            return false;
        }
    }
}