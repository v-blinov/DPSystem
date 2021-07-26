namespace ApiDPSystem.Entities
{
    public class FeatureType
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public Feature Feature { get; set; }
    }
}
