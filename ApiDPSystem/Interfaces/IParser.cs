using ApiDPSystem.Models.Parser;

namespace ApiDPSystem.Interfaces
{
    public interface IParser<T>
    {
        public Root<T> DeserializeFile(string fileContent);

        public void SetDataToDatabase(Root<T> data);
    }
}
