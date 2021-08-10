﻿using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ApiDPSystem.Entities;
using ApiDPSystem.Models.Parser;
using ApiDPSystem.Repository;
using Microsoft.AspNetCore.Http;

namespace ApiDPSystem.Services
{
    public class FileService
    {
        private readonly MapperRepository _mapperRepository;

        public FileService(MapperRepository mapperRepository)
        {
            _mapperRepository = mapperRepository;
        }


        public async Task ProcessFileAsync(IFormFile file, string dealer)
        {
            //var fileExtension = Path.GetExtension(file.FileName);

            //var fileContent = await ReadFileAsync(file);

            //var ProcessedDbModels = fileExtension switch {
            //    ".json" => ProcessJsonWithVersion(fileContent, dealer),
            //    ".xml" => ProcessXmlWithVersion(fileContent, dealer),
            //    ".yaml" => ProcessYamlWithVersion(fileContent, dealer),
            //    ".csv" => ProcessCsvWithVersion(fileContent, file.FileName, dealer),
            //    _ => throw new Exception("Unknown file format")
            //};

            //_mapperRepository.TransferSoldCars(ProcessedDbModels, dealer);
            //SetToDatabase(ProcessedDbModels);

            //////////////////////////////////////////////////////////////////////////////////////////////////////////////

            var fileContent = await ReadFileAsync(file);
            var fileExtension = Path.GetExtension(file.FileName);

            var parser = Selector.GetParser(fileExtension);
            var dDbModels = parser.Parse(fileContent, file.FileName, dealer);

            _mapperRepository.TransferSoldCars(dDbModels, dealer);
            SetToDatabase(dDbModels);
        }

        private async Task<string> ReadFileAsync(IFormFile file)
        {
            using var reader = new StreamReader(file.OpenReadStream());
            return await reader.ReadToEndAsync();
        }

        //private List<Entities.CarActual> ProcessJsonWithVersion(string fileContent, string dealer)
        //{
        //    var version = new Distributer().JsonGetVersion(fileContent);
        //    var dbModels = new List<Entities.CarActual>();

        //    //switch (version.Value)
        //    //{
        //    //    case 1:
        //    //        var jsonParser_v1 = new JsonParser<FileFormat.Json.Version1.Car>();
        //    //        var deserializeJsonModel = jsonParser_v1.DeserializeFile(fileContent);
        //    //        dbModels = jsonParser_v1.MapToDBModel(deserializeJsonModel, dealer);
        //    //        break;
        //    //    default:
        //    //        throw new Exception($"Unknown Json file version {version.Value}");
        //    //}

        //    return dbModels;
        //}

        //private List<Entities.CarActual> ProcessXmlWithVersion(string fileContent, string dealer)
        //{
        //    var version = new Distributer().XmlGetVersion(fileContent);
        //    var dbModels = new List<Entities.CarActual>();

        //    switch (version.Value)
        //    {
        //        case 1:
        //            var xmlParser_v1 = new XmlParser<FileFormat.Xml.Version1.Car>();
        //            var deserializeXmlModel = xmlParser_v1.DeserializeFile(fileContent);
        //            dbModels = xmlParser_v1.MapToDBModel(deserializeXmlModel, dealer);
        //            break;
        //        default:
        //            throw new Exception($"Unknown Xml file version {version.Value}");
        //    }

        //    return dbModels;
        //}

        //private List<Entities.CarActual> ProcessYamlWithVersion(string fileContent, string dealer)
        //{
        //    var version = new Distributer().YamlGetVersion(fileContent);
        //    var dbModels = new List<Entities.CarActual>();

        //    switch (version.Value)
        //    {
        //        case 1:
        //            var yamlParser_v1 = new YamlParser<FileFormat.Yaml.Version1.Car>();
        //            var deserializeYamlModel = yamlParser_v1.DeserializeFile(fileContent);
        //            dbModels = yamlParser_v1.MapToDBModel(deserializeYamlModel, dealer);
        //            break;
        //        default:
        //            throw new Exception($"Unknown Yaml file version {version.Value}");
        //    }

        //    return dbModels;
        //}

        //private List<Entities.CarActual> ProcessCsvWithVersion(string fileContent, string fileName, string dealer) 
        //{
        //    var version = new Distributer().CsvGetVersion(fileName);
        //    var dbModels = new List<Entities.CarActual>();

        //    switch (version.Value)
        //    {
        //        case 1:
        //            var csvParser_v1 = new CsvParser<FileFormat.Csv.Version1.Car>();
        //            var deserializeCsvModel_v1 = csvParser_v1.DeserializeFile_V1(fileContent);
        //            dbModels = csvParser_v1.MapToDBModel(deserializeCsvModel_v1, dealer);
        //            break;
        //        case 2:
        //            var csvParser_v2 = new CsvParser<FileFormat.Csv.Version2.Car>();
        //            var deserializeCsvModel_v2 = csvParser_v2.DeserializeFile_V2(fileContent);
        //            dbModels = csvParser_v2.MapToDBModel(deserializeCsvModel_v2, dealer);
        //            break;
        //        default:
        //            throw new Exception($"Unknown Yaml file version {version.Value}");
        //    }

        //    return dbModels;
        //}


        private void SetToDatabase(List<CarActual> models)
        {
            foreach (var model in models)
            {
                var isUsingExistedConfiguration = SetConfigurationIdIfExist(model);
                if (!isUsingExistedConfiguration)
                    SetConfigurationFeatureIdsIfExist(model);

                SetColorIdIfExist(model);
                SetCarImageIdsIfExist(model);
                SetDealerIdIfExist(model);

                var existedCar = _mapperRepository.GetThatDealerCarIfExist(model);

                if (existedCar == null)
                {
                    _mapperRepository.AddCarToDb(model);
                    continue;
                }

                var isModified = IsCarModified(model, existedCar);
                if (isModified)
                {
                    _mapperRepository.TransferOneCar(existedCar);
                    _mapperRepository.AddCarToDb(model);
                }
            }
        }

        private bool SetConfigurationIdIfExist(CarActual model)
        {
            var exitedConfiguration = _mapperRepository.ReturnConfigurationIfExist(model.Configuration);
            if (exitedConfiguration != null)
            {
                model.Configuration = null;
                model.ConfigurationId = exitedConfiguration.Id;
                return true;
            }

            var existedProducer = _mapperRepository.ReturnProducerIfExist(model.Configuration.Producer);
            if (existedProducer != null)
            {
                model.Configuration.Producer = null;
                model.Configuration.ProducerId = existedProducer.Id;
            }

            var existedEngine = _mapperRepository.ReturnEngineIfExist(model.Configuration.Engine);
            if (existedEngine != null)
            {
                model.Configuration.Engine = null;
                model.Configuration.EngineId = existedEngine.Id;
            }

            return false;
        }

        private void SetConfigurationFeatureIdsIfExist(CarActual model)
        {
            foreach (var configurationFeature in model.Configuration.ConfigurationFeatures)
            {
                var existedFeature = _mapperRepository.ReturnFeatureIfExist(configurationFeature.Feature);
                if (existedFeature != null)
                {
                    configurationFeature.Feature = null;
                    configurationFeature.FeatureId = existedFeature.Id;
                }
            }
        }

        private void SetColorIdIfExist(CarActual model)
        {
            var existedInteriorColor = _mapperRepository.ReturnColorIfExist(model.InteriorColor);
            if (existedInteriorColor != null)
            {
                model.InteriorColor = null;
                model.InteriorColorId = existedInteriorColor.Id;
            }

            var existedExteriorColor = _mapperRepository.ReturnColorIfExist(model.ExteriorColor);
            if (existedExteriorColor != null)
            {
                model.ExteriorColor = null;
                model.ExteriorColorId = existedExteriorColor.Id;
            }

            if (model.ExteriorColor != null && model.ExteriorColor.Equals(model.InteriorColor))
                model.ExteriorColor = model.InteriorColor;
        }

        private void SetCarImageIdsIfExist(CarActual model)
        {
            foreach (var carImage in model.CarImages)
            {
                var existedImage = _mapperRepository.ReturnImageIfExist(carImage.Image);
                if (existedImage != null)
                {
                    carImage.Image = null;
                    carImage.ImageId = existedImage.Id;
                }
            }
        }

        private void SetDealerIdIfExist(CarActual model)
        {
            var existedDealer = _mapperRepository.ReturnDealerIfExist(model.Dealer);
            if (existedDealer != null)
            {
                model.Dealer = null;
                model.DealerId = existedDealer.Id;
            }
        }

        private bool IsCarModified(CarActual newCar, CarActual existedCar)
        {
            if (newCar.ConfigurationId != existedCar.ConfigurationId) return true;
            if (newCar.ExteriorColorId != existedCar.ExteriorColorId) return true;
            if (newCar.InteriorColorId != existedCar.InteriorColorId) return true;
            if (newCar.Price != existedCar.Price) return true;

            var existedCarImages = _mapperRepository.GetCarImagesListByCarId(existedCar.Id);
            if (newCar.CarImages.Count != existedCarImages.Count) return true;

            var existedCarImagesIds = existedCarImages.OrderBy(p => p.ImageId).Select(p => p.ImageId).ToList();
            var newCarImagesIds = newCar.CarImages.OrderBy(p => p.ImageId).Select(p => p.ImageId).ToList();
            if (!existedCarImagesIds.SequenceEqual(newCarImagesIds)) return true;

            return false;
        }
    }
}