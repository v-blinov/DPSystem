using System.Collections.Generic;
using YamlDotNet.Serialization;

namespace ApiDPSystem.FileFormat.Yaml.Version1
{
    public class OtherOptions
    {
        [YamlMember(Alias = "exterior")]
        public List<string> Exterior { get; set; }

        [YamlMember(Alias = "interior")]
        public List<string> Interior { get; set; }

        [YamlMember(Alias = "safety")]
        public List<string> Safety { get; set; }
    }
}
