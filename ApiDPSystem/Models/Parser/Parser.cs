using Microsoft.AspNetCore.Http;

namespace ApiDPSystem.Models.Parser
{
    public abstract class Parser
    {
        public abstract void ProcessFile(IFormFile file);
    }
}
