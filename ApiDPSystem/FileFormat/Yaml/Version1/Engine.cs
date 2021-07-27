using YamlDotNet.Serialization;

namespace ApiDPSystem.FileFormat.Yaml.Version1
{
    public class Engine
    {
        [YamlMember(Alias = "fuel")]
        public string Fuel { get; set; }

        [YamlMember(Alias = "power")]
        public string Power { get; set; }

        [YamlMember(Alias = "capacity")]
        public string Capacity { get; set; }
    }
}
