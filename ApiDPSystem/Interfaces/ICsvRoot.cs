namespace ApiDPSystem.Interfaces
{
    interface ICsvRoot : IRoot
    {
        public IRoot Deserialize(string fileContent);
    }
}
