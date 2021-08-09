using System.Collections.Generic;

namespace ApiDPSystem.Models
{
    public class ApiResponse
    {
        public ApiResponse()
        {
            Message = string.Empty;
            Errors = new List<string>();
        }

        public int StatusCode { get; set; }
        public string Message { get; set; }
        public bool IsSuccess { get; set; }
        public List<string> Errors { get; set; }
    }

    public class ApiResponse<T> : ApiResponse
    {
        public T Content { get; set; }
    }
}