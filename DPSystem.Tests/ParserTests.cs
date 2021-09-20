using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ApiDPSystem.Entities;
using ApiDPSystem.Exceptions;
using ApiDPSystem.Models.Parser;
using FluentAssertions;
using Xunit;

namespace DPSystem.Tests
{
    public class ParserTests
    {
        private const string DefaultDealer = "defaultDealer";

        private readonly List<Car> _expectedContentResult = new()
        {
            new Car
            {
                VinCode = "ABCDEFGH",
                Dealer = new Dealer { Name = DefaultDealer },
                Configuration = new Configuration
                {
                    Year = 2021,
                    Producer = new Producer { Name = "Test" },
                    Model = "TestModel",
                    ModelTrim = "ABC",
                    Engine = new Engine { Fuel = "Benzin", Power = 500, Capacity = 500 },
                    Transmission = "MT",
                    Drive = "AWD"
                },
                CarFeatures = new List<CarFeature>
                {
                    new() { Feature = new Feature { Type = "Safety", Description = "Система контроля слепых зон" } }
                },
                ExteriorColor = new Color { Name = "Marina blue" },
                InteriorColor = new Color { Name = "White" },
                CarImages = new List<CarImage> { new() { Image = new Image { Url = "/test/url.png" } } },
                Price = 1000000
            }
        };


        [InlineData(".json", typeof(JsonParser))]
        [InlineData(".csv", typeof(CsvParser))]
        [InlineData(".yaml", typeof(YamlParser))]
        [InlineData(".xml", typeof(XmlParser))]
        [Theory]
        public void Choose_parser_by_file_extension_positive(string fileExtension, Type parserType)
        {
            var parser = Selector.GetParser(fileExtension);

            parser.Should().BeOfType(parserType);
        }


        [InlineData("Correct/CorrectJson2.json", 5)]
        [InlineData("Correct/CorrectJson3.json", 2)]
        [Theory]
        public void Parse_correct_json_file(string fileName, int resultCarsCount)
        {
            // Arrange
            var fileContent = ReadFile(GetBasePath() + fileName);
            var sut = new JsonParser();

            // Act
            var parsingResult = sut.Parse(fileContent, fileName, DefaultDealer);

            // Assert
            parsingResult.Should().HaveCount(resultCarsCount);
            parsingResult.Should().BeOfType<List<Car>>();
        }

        [InlineData("Correct/CorrectCsv1_v1.csv", 1)]
        [InlineData("Correct/CorrectCsv2_v2.csv", 2)]
        [Theory]
        public void Parse_correct_csv_file(string fileName, int resultCarsCount)
        {
            // Arrange
            var fileContent = ReadFile(GetBasePath() + fileName);
            var sut = new CsvParser();

            // Act
            var parsingResult = sut.Parse(fileContent, fileName, DefaultDealer);

            // Assert
            parsingResult.Should().HaveCount(resultCarsCount);
            parsingResult.Should().BeOfType<List<Car>>();
        }

        [InlineData("Correct/CorrectYaml1.yaml", 1)]
        [Theory]
        public void Parse_correct_yaml_file(string fileName, int resultCarsCount)
        {
            // Arrange
            var fileContent = ReadFile(GetBasePath() + fileName);
            var sut = new YamlParser();

            // Act
            var parsingResult = sut.Parse(fileContent, fileName, DefaultDealer);

            // Assert
            parsingResult.Should().HaveCount(resultCarsCount);
            parsingResult.Should().BeOfType<List<Car>>();
        }

        [InlineData("Correct/CorrectXml1.xml", 1)]
        [Theory]
        public void Parse_correct_xml_file(string fileName, int resultCarsCount)
        {
            // Arrange
            var fileContent = ReadFile(GetBasePath() + fileName);
            var sut = new XmlParser();

            // Act
            var parsingResult = sut.Parse(fileContent, fileName, DefaultDealer);

            // Assert
            parsingResult.Should().HaveCount(resultCarsCount);
            parsingResult.Should().BeOfType<List<Car>>();
        }


        [Fact]
        public void Check_correct_json_content()
        {
            // Arrange
            const string fileName = "Correct/DefaultJson.json";
            var fileContent = ReadFile(GetBasePath() + fileName);
            var sut = new JsonParser();

            // Act
            var parsingResult = sut.Parse(fileContent, fileName, DefaultDealer);

            //Assert
            parsingResult.Should().BeOfType<List<Car>>();
            IsModelsEqual(_expectedContentResult, parsingResult).Should().BeTrue();
        }

        [Fact]
        public void Check_correct_xml_content()
        {
            // Arrange
            const string fileName = "Correct/DefaultXml.xml";
            var fileContent = ReadFile(GetBasePath() + fileName);
            var sut = new XmlParser();

            // Act
            var parsingResult = sut.Parse(fileContent, fileName, DefaultDealer);

            //Assert
            parsingResult.Should().BeOfType<List<Car>>();
            IsModelsEqual(_expectedContentResult, parsingResult).Should().BeTrue();
        }

        [Fact]
        public void Check_correct_yaml_content()
        {
            // Arrange
            const string fileName = "Correct/DefaultYaml.yaml";
            var fileContent = ReadFile(GetBasePath() + fileName);
            var sut = new YamlParser();

            // Act
            var parsingResult = sut.Parse(fileContent, fileName, DefaultDealer);

            //Assert
            parsingResult.Should().BeOfType<List<Car>>();
            IsModelsEqual(_expectedContentResult, parsingResult).Should().BeTrue();
        }

        [Fact]
        public void Check_correct_csv_content()
        {
            // Arrange
            const string fileName = "Correct/DefaultCsv_v1.csv";
            var fileContent = ReadFile(GetBasePath() + fileName);
            var sut = new CsvParser();

            // Act
            var parsingResult = sut.Parse(fileContent, fileName, DefaultDealer);

            //Assert
            parsingResult.Should().BeOfType<List<Car>>();
            IsModelsEqual(_expectedContentResult, parsingResult).Should().BeTrue();
        }


        [Fact]
        public void Try_parse_invalid_json_file()
        {
            // Arrange
            var fileName = "Invalid/InvalidJson1.json";
            var fileContent = ReadFile(GetBasePath() + fileName);
            var sut = new JsonParser();

            // Assert
            Assert.Throws<InvalidFileException>(() => sut.Parse(fileContent, fileName, DefaultDealer));
        }

        [Fact]
        public void Try_parse_invalid_xml_file()
        {
            // Arrange
            var fileName = "Invalid/InvalidXml1.xml";
            var fileContent = ReadFile(GetBasePath() + fileName);
            var sut = new XmlParser();

            // Assert
            Assert.Throws<InvalidFileException>(() => sut.Parse(fileContent, fileName, DefaultDealer));
        }

        [Fact]
        public void Try_parse_invalid_yaml_file()
        {
            // Arrange
            var fileName = "Invalid/InvalidYaml1.yaml";
            var fileContent = ReadFile(GetBasePath() + fileName);
            var sut = new YamlParser();

            // Assert
            Assert.Throws<InvalidFileException>(() => sut.Parse(fileContent, fileName, DefaultDealer));
        }


        private static string GetBasePath() =>
            @"..\..\..\TestFiles\";
        private static string ReadFile(string path)
        {
            using var reader = new StreamReader(path);
            return reader.ReadToEnd();
        }
        private static bool IsModelsEqual(List<Car> expectedCarActuals, List<Car> testCarActuals)
        {
            for (var i = 0; i < expectedCarActuals.Count; i++)
            {
                var expectedCar = expectedCarActuals[i];
                var testCar = testCarActuals[i];

                if (expectedCar.VinCode != testCar.VinCode) return false;
                if (!expectedCar.Configuration.Equals(testCar.Configuration)) return false;

                if (expectedCar.CarFeatures.Count() != testCar.CarFeatures.Count()) return false;

                var expectedFeatures = expectedCar.CarFeatures.ToList();
                var testResultFeatures = testCar.CarFeatures.ToList();

                for (var j = 0; j < expectedFeatures.Count; j++)
                    if (expectedFeatures[j].Feature.Description != testResultFeatures[j].Feature.Description &&
                        expectedFeatures[j].Feature.Type != testResultFeatures[j].Feature.Type)
                        return false;

                if (!expectedCar.ExteriorColor.Equals(testCar.ExteriorColor)) return false;
                if (!expectedCar.InteriorColor.Equals(testCar.InteriorColor)) return false;
                if (expectedCar.CarImages.Count() != testCar.CarImages.Count()) return false;

                var expectedImages = expectedCar.CarImages.ToList();
                var testResultImages = testCar.CarImages.ToList();

                for (var j = 0; j < expectedImages.Count; j++)
                    if (expectedImages[j].Image.Url != testResultImages[j].Image.Url)
                        return false;

                if (expectedCar.Price != testCar.Price) return false;
            }
            return true;
        }
    }
}