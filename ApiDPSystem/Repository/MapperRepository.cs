using ApiDPSystem.Data;
using ApiDPSystem.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace ApiDPSystem.Repository
{
    public class MapperRepository
    {
        private readonly Context _context;

        public MapperRepository(Context context)
        {
            _context = context;
        }

        public Configuration ReturnConfigurationIfExist(Configuration configuration)
        {
            if (configuration == null)
                return configuration;

            return _context.Configurations
                           .Include(p => p.Engine)
                           .Include(p => p.Producer)
                           .ToList()
                           .FirstOrDefault(p => configuration.Equals(p));
        }
        public Producer ReturnProducerIfExist(Producer producer)
        {
            if (producer == null)
                return producer;

            return _context.Producers
                           .ToList()
                           .FirstOrDefault(p => producer.Equals(p));
        }
        public Engine ReturnEngineIfExist(Engine engine)
        {
            if (engine == null)
                return engine;

            return _context.Engines
                           .ToList()
                           .FirstOrDefault(p => engine.Equals(p));
        }
        public Feature ReturnFeatureIfExist(Feature feature)
        {
            if (feature == null)
                return feature;

            return _context.Features
                           .ToList()
                           .FirstOrDefault(p => feature.Equals(p));
        }
        public Color ReturnColorIfExist(Color color)
        {
            if (color == null)
                return color;

            return _context.Colors
                           .ToList()
                           .FirstOrDefault(p => color.Equals(p));
        }
        public Image ReturnImageIfExist(Image image)
        { 
            if (image == null)
                return image;

            return _context.Images
                           .ToList()
                           .FirstOrDefault(p => image.Equals(p));
        }
        public Dealer ReturnDealerIfExist(Dealer dealer)
        {
            if (dealer == null)
                return dealer;

            return _context.Dealers
                           .ToList()
                           .FirstOrDefault(p => dealer.Equals(p));
        }

        public async Task AddCarEntityOrUpdateIfExist(CarEntity model)
        {
            var existedCar = _context.CarEntities.FirstOrDefault(p => p.IsAvailable && p.VinCode == model.VinCode && (p.Dealer == model.Dealer || p.DealerId == model.DealerId));

            if (existedCar == null)
                _context.CarEntities.Add(model);
            else
            {
                existedCar.CarImages = model.CarImages;
                existedCar.Configuration = model.Configuration;
                existedCar.ConfigurationId = model.ConfigurationId;
                existedCar.Dealer = model.Dealer;
                existedCar.DealerId = model.DealerId;
                existedCar.ExteriorColor = model.ExteriorColor;
                existedCar.ExteriorColorId = model.ExteriorColorId;
                existedCar.InteriorColor = model.InteriorColor;
                existedCar.InteriorColorId = model.InteriorColorId;
                existedCar.Price = model.Price;

                _context.Entry(existedCar).State = EntityState.Modified;
            }

            await _context.SaveChangesAsync();
        }
    }
}
