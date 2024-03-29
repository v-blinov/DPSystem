﻿using System.Collections.Generic;

namespace ApiDPSystem.Entities
{
    public class Feature
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public string Description { get; set; }

        public ICollection<CarFeature> CarFeatures { get; set; }


        public override bool Equals(object obj)
        {
            if (obj is not Feature feature)
                return false;

            return feature.Type == Type && feature.Description == Description;
        }

        public override int GetHashCode() =>
            Type.GetHashCode() * 22 + Description.GetHashCode() * 13;

        public Feature GetValuesCopy() =>
            new()
            {
                Description = Description,
                Type = Type
            };
    }
}