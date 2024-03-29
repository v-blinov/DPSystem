﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ApiDPSystem.Entities
{
    public class Image
    {
        public int Id { get; set; }

        [Url]
        public string Url { get; set; }

        public ICollection<CarImage> CarImages { get; set; }


        public override bool Equals(object obj)
        {
            if (obj is not Image image)
                return false;

            return image.Url == Url;
        }

        public override int GetHashCode() =>
            Url.GetHashCode() * 22;

        public Image GetValuesCopy() =>
            new()
            {
                Url = Url
            };
    }
}