using System.IO;

namespace Common.ComandManager.Verifiers
{
    public class SourceFileExistingVerifier : IVerifier
    {
        public bool TryVerify(string[] args, out string errorMessage)
        {
            errorMessage = string.Empty;
            if (File.Exists(args[1]))
                return true;

            errorMessage = $"Source file {args[1]} not exist";
            return false;
        }
    }

    public class TargetFileExistingVerifier : IVerifier
    {
        public bool TryVerify(string[] args, out string errorMessage)
        {
            errorMessage = string.Empty;
            if (!File.Exists(args[2]))
                return true;

            errorMessage = $"Target file {args[2]} allready exist";
            return false;
        }
    }
}