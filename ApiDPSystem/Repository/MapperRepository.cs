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

        public Color getColor(string name)
        {
            var color = _context.Colors.FirstOrDefault(p => p.Name == name);

            if (color == null)
            {
                color = new Color { Name = name };
                _context.Colors.Add(color);
                _context.SaveChanges();
            }
             
            return color;
        }

        public FeatureType getFeatureType(string name)
        {
            var featureType = _context.FeatureTypes.FirstOrDefault(p => p.Name == name);

            if (featureType == null)
            {
                featureType = new FeatureType { Name = name };
                _context.FeatureTypes.Add(featureType);
                _context.SaveChanges();
            }

            return featureType;
        }

        public List<Feature> AddFeatures(List<string> features, string name)
        {
            var featureType = getFeatureType(name);

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

        public void AddCarFeatureNotes(List<Feature> features, Car car)
        {
            foreach (var feature in features)
            {
                _context.CarFeatures.Add(new CarFeature { Feature = feature, Car = car });
            }

            _context.SaveChanges();
        }
    }
}
