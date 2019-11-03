namespace Common.ComandManager.Verifiers
{
    public interface IVerifier
    {
        bool TryVerify(string[] args, out string errorMessage);
    }
}