using ApiDPSystem.Data;
using ApiDPSystem.Entities;
using System.Collections.Generic;
using System.Linq;

namespace ApiDPSystem.Repository
{
    public class MapperRepository
    {
        private readonly Context _context;

        public MapperRepository(Context context)
        {
            _context = context;
        }


        public FeatureType GetFeatureType(string name) =>
            _context.FeatureTypes.FirstOrDefault(p => p.Name == name);

        public List<Feature> GetFeature(List<string> features, string name)
        {
            var featureType = GetFeatureType(name);
            

            var storedFeatures = new List<Feature>();

            foreach (var feature in features)
                storedFeatures.Add(new Feature
                {
                    FeatureType = featureType,
                    Description = feature
                });

            _context.Features.AddRange(storedFeatures);

            return storedFeatures;
        }

        public Transmission GetTransmission(string value) =>
            _context.Transmissions.FirstOrDefault(p => p.Value == value);

        public Producer GetProducer(string name) =>
            _context.Producers.FirstOrDefault(p => p.Name == name);

        public Engine GetEngine(Engine currentEngine) =>
            _context.Engines.FirstOrDefault(p => p.Equals(currentEngine));

        public Color getColor(string name) =>
            _context.Colors.FirstOrDefault(p => p.Name == name);

    }
}
