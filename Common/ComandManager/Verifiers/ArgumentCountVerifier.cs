namespace Common.ComandManager.Verifiers
{
    public class ArgumentCountVerifier : IVerifier
    {
        public bool TryVerify(string[] args, out string errorMessage)
        {
            errorMessage = string.Empty;
            if (args != null && args.Length == 3)
                return true;

            errorMessage = "Input parameters not valid";
            return false;
        }
    }
}