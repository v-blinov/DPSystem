using YamlDotNet.Serialization;

namespace ApiDPSystem.FileFormat.Yaml.UniversalReadVersion
{
    public record Colors()
    {
        [YamlMember(Alias = "interior")]
        public string Interior { get; set; }

        [YamlMember(Alias = "exterior")]
        public string Exterior { get; set; }
    }
}