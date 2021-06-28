using System;
using System.Collections.Generic;

namespace ApiDPSystem.Models
{
    public class ApiResponse<T>
    {
        public ApiResponse()
        {
            Message = String.Empty;
            Errors = new List<string>();
        }

        public int StatusCode { get; set; }
        public string Message { get; set; }
        public bool IsSuccess { get; set; }
        public List<string> Errors { get; set; }
        public T Content { get; set; }
    }
}
