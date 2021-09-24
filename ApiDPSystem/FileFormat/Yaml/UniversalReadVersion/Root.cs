using System.Collections.Generic;
using YamlDotNet.Serialization;

namespace ApiDPSystem.FileFormat.Yaml.UniversalReadVersion
{
    public record Root()
    {
        [YamlMember(Alias = "cars")]
        public List<Car> Cars { get; set; }
    }
}