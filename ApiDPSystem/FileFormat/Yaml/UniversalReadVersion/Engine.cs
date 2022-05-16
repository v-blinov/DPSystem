using YamlDotNet.Serialization;

namespace ApiDPSystem.FileFormat.Yaml.UniversalReadVersion
{
    public record Engine
    {
        [YamlMember(Alias = "fuel")]
        public string Fuel { get; set; }

        [YamlMember(Alias = "power")]
        public string Power { get; set; }

        [YamlMember(Alias = "capacity")]
        public string Capacity { get; set; }
    }
}