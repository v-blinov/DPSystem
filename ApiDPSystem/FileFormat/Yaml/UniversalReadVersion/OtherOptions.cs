using System.Collections.Generic;
using YamlDotNet.Serialization;

namespace ApiDPSystem.FileFormat.Yaml.UniversalReadVersion
{
    public record OtherOptions()
    {
        [YamlMember(Alias = "exterior")]
        public List<string> Exterior { get; set; }

        [YamlMember(Alias = "interior")]
        public List<string> Interior { get; set; }

        [YamlMember(Alias = "safety")]
        public List<string> Safety { get; set; }
    }
}