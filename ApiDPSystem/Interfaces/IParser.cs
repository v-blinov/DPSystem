using Microsoft.AspNetCore.Http;

namespace ApiDPSystem.Interfaces
{
    public interface IParser<T>
    {
        public Root<T> DeserializeFile(IFormFile file);

        public void SetDataToDatabase(Root<T> data);
    }
}
