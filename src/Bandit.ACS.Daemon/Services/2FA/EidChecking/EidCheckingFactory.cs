namespace Bandit.ACS.Daemon.Services.EidChecking
{
    public class EidCheckingFactory
    {
        public IEidCheckingAlgorithm? Create(string algorithm)
            => algorithm switch
            {
                "SHA256withRSA" => new RSASha256EIDAlgorithm(),
                "SHA384withECDSA" => new ECSha384EIDAlgorithm(),
                _ => null
            };
    }
}
