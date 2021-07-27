using YamlDotNet.Serialization;

namespace ApiDPSystem.FileFormat.Yaml.Version1
{
    public class TechincalOptions
    {
        [YamlMember(Alias = "engine")]
        public Engine Engine { get; set; }

        [YamlMember(Alias = "transmission")]
        public string Transmission { get; set; }

        [YamlMember(Alias = "drive")]
        public string Drive { get; set; }
    }
}
