﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ApiDPSystem.Models
{
    public class AuthenticationResult
    {
        public string Token { get; set; }
        public string RefreshToken { get; set; }
        public bool Success { get; set; }
        public List<string> Errors { get; set; }
    }
}