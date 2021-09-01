﻿using System;

namespace ApiDPSystem.Exceptions
{
    [System.Serializable]
    public class InvalidFileVersionException : Exception
    {
        public int InvalidVersion { get; set; }
        public string FileFormat { get; set; }
        
        public InvalidFileVersionException() : this($"Файл не поддерживается.") { }
        public InvalidFileVersionException(string message) : base(message) { }
        public InvalidFileVersionException(string message, Exception inner) : base(message, inner) { }
        public InvalidFileVersionException(string message, string fileFormat, int invalidVersion) : base(message)
        {
            FileFormat = fileFormat;
            InvalidVersion = invalidVersion;
        }
    }
}
