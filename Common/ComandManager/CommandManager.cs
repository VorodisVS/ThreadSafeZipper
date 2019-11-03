using System;
using System.Collections.Generic;
using System.Reflection;
using Common.ComandManager.Commands;
using Common.ComandManager.Verifiers;

namespace Common.ComandManager
{
    public class CommandManager
    {
        private static readonly List<IVerifier> _verifiers;
        private static readonly Dictionary<string, Type> _avialbeCommands;

        static CommandManager()
        {
            _avialbeCommands = new Dictionary<string, Type>
            {
                {"Decompress", typeof(DecompressCommand)},
                {"Compress", typeof(CompressCommand)}
            };

            _verifiers = new List<IVerifier>
            {
                new ArgumentCountVerifier(),
                new CommandValueVerifier(_avialbeCommands.Keys),
                new SourceFileExistingVerifier(),
                new TargetFileExistingVerifier()
            };
        }

        public static bool Verify(string[] args, out string errMessage)
        {
            errMessage = string.Empty;

            foreach (var verifier in _verifiers)
                if (!verifier.TryVerify(args, out var message))
                {
                    errMessage = message;
                    return false;
                }

            return true;
        }

        public static ICommand GetCommand(string[] args)
        {
            object[] objs = new object[args.Length];
            for (int i = 0 ; i < args.Length; i++)
            {
                objs[i] = args[i];
            }
            
            return Activator.CreateInstance(_avialbeCommands[args[0]], args) as ICommand;
        }
    }
}