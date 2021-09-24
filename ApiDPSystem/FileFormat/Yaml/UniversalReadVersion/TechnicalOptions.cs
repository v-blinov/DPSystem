using YamlDotNet.Serialization;

namespace ApiDPSystem.FileFormat.Yaml.UniversalReadVersion
{
    public record TechnicalOptions()
    {
        [YamlMember(Alias = "engine")]
        public Engine Engine { get; set; }

        [YamlMember(Alias = "transmission")]
        public string Transmission { get; set; }

        [YamlMember(Alias = "drive")]
        public string Drive { get; set; }
    }
}