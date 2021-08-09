using System;
using System.Collections.Generic;
using System.Linq;
using ApiDPSystem.Interfaces;

namespace ApiDPSystem.Models.Parser
{
    public static class Selector
    {
        private static readonly Dictionary<string, Type> _parserTypes;
        private static readonly Dictionary<(string, int), Type> _modelTypes;

        static Selector()
        {
            //Select all classes that implement IBParser interface 
            var parserType = typeof(IBParser);
            var parserTypeNames = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(p => p.GetTypes())
                .Where(p => parserType.IsAssignableFrom(p) && p.IsClass && !p.IsAbstract);

            _parserTypes = new Dictionary<string, Type>();
            foreach (var type in parserTypeNames)
            {
                var instance = Activator.CreateInstance(type) as IBParser;
                _parserTypes.Add(instance.ConvertableFileExtension, type);
            }

            //Select all classes that implement IRoot interface 
            var modeltype = typeof(IRoot);
            var modelTypeNames = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(p => p.GetTypes())
                .Where(p => modeltype.IsAssignableFrom(p) && p.IsClass && !p.IsAbstract);

            _modelTypes = new Dictionary<(string, int), Type>();
            foreach (var type in modelTypeNames)
            {
                var instance = Activator.CreateInstance(type) as IRoot;
                _modelTypes.Add((instance.FileFormat, instance.Version), type);
            }
        }

        public static IBParser GetParser(string fileExtension)
        {
            var parserType = _parserTypes[fileExtension];
            return Activator.CreateInstance(parserType) as IBParser;
        }

        public static Type GetResultType(string fileExtension, int version)
        {
            return _modelTypes[(fileExtension, version)];
        }
    }
}