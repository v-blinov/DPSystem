namespace ApiDPSystem.Interfaces
{
    internal interface ICsvRoot : IRoot
    {
        public IRoot Deserialize(string fileContent);
    }
}