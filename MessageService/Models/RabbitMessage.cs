using System.Collections.Generic;

namespace MessageService.Models
{
    public class RabbitMessage
    {
        public RabbitMessage()
        {
            Addresses = new List<string>();
        }

        public List<string> Addresses { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }
    }
}
