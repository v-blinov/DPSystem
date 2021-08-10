using System;
using System.Collections.Generic;
using System.Linq;
using ApiDPSystem.Interfaces;

namespace ApiDPSystem.Models.Parser
{
    public static class Selector
    {
        private static readonly Dictionary<string, Type> ParserTypes;
        private static readonly Dictionary<(string, int), Type> ModelTypes;

        static Selector()
        {
            //Select all classes that implement IBParser interface 
            var parserType = typeof(IBParser);
            var parserTypeNames = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(p => p.GetTypes())
                .Where(p => parserType.IsAssignableFrom(p) && p.IsClass && !p.IsAbstract);

            ParserTypes = new Dictionary<string, Type>();
            foreach (var type in parserTypeNames)
            {
                var instance = Activator.CreateInstance(type) as IBParser;
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

        public static IBParser GetParser(string fileExtension)
        {
            var parserType = ParserTypes[fileExtension];
            return Activator.CreateInstance(parserType) as IBParser;
        }

        public static Type GetResultType(string fileExtension, int version)
        {
            return ModelTypes[(fileExtension, version)];
        }
    }
}