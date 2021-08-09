using YamlDotNet.Serialization;

namespace ApiDPSystem.FileFormat.Yaml.Version1
{
    public class Colors
    {
        [YamlMember(Alias = "interior")]
        public string Interior { get; set; }

        [YamlMember(Alias = "exterior")]
        public string Exterior { get; set; }
    }
}