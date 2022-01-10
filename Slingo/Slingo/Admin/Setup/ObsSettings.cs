namespace Slingo.Admin.Setup
{
    public class ObsSettings
    {
        public string ObsAddress { get; }
        public string ObsPassword { get; }

        public ObsSettings(string obsAddress, string obsPassword)
        {
            ObsAddress = obsAddress;
            ObsPassword = obsPassword;
        }
    }
}