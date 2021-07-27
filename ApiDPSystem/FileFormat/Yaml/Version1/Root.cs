using ApiDPSystem.Interfaces;
using System.Collections.Generic;
using YamlDotNet.Serialization;

namespace ApiDPSystem.FileFormat.Yaml.Version1
{
    public class Root : IFormat<Car>
    {
        [YamlMember(Alias = "version")]
        public int Version { get; set; }

        [YamlMember(Alias = "cars")]
        public List<Car> Cars { get; set; }
    }
}
