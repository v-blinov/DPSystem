﻿using System;
using System.Collections.Generic;
using System.Linq;
using ApiDPSystem.Exceptions;
using ApiDPSystem.Interfaces;

namespace ApiDPSystem.Models.Parser
{
    public static class Selector
    {
        private static readonly Dictionary<string, Type> ParserTypes;
        private static readonly Dictionary<(string, int), Type> ModelTypes;

        static Selector()
        {
            //Select all classes that implement IParser interface 
            var parserType = typeof(IParser);
            var parserTypeNames = AppDomain.CurrentDomain.GetAssemblies()
                                           .SelectMany(p => p.GetTypes())
                                           .Where(p => parserType.IsAssignableFrom(p) && p.IsClass && !p.IsAbstract);

            ParserTypes = new Dictionary<string, Type>();
            foreach (var type in parserTypeNames)
            {
                var instance = Activator.CreateInstance(type) as IParser;
                ParserTypes.Add(instance.ConvertableFileExtension, type);
            }


            //Select all classes that implement IRoot interface 
            var modelType = typeof(IRoot);
            var modelTypeNames = AppDomain.CurrentDomain.GetAssemblies()
                                          .SelectMany(p => p.GetTypes())
                                          .Where(p => modelType.IsAssignableFrom(p) && p.IsClass && !p.IsAbstract);

            ModelTypes = new Dictionary<(string, int), Type>();
            foreach (var type in modelTypeNames)
            {
                var instance = Activator.CreateInstance(type) as IRoot;
                ModelTypes.Add((instance.FileFormat, instance.Version), type);
            }
        }

        public static IParser GetParser(string fileExtension)
        {
            var parserType = ParserTypes[fileExtension];
            return Activator.CreateInstance(parserType) as IParser;
        }

        public static Type GetResultType(string fileExtension, int version)
        {
            if (ModelTypes.TryGetValue((fileExtension, version), out var type))
                return type;
            throw new InvalidFileVersionException($"Неподдерживаемый файл фомата {fileExtension} версии {version}", fileExtension, version);
        }
    }
}