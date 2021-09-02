using ApiDPSystem.Entities;
using ApiDPSystem.Exceptions;
using ApiDPSystem.Models.Parser;
using AutoFixture;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Moq;
using System;
using System.Collections.Generic;
using System.IO;
using Xunit;

namespace DPSystem.Tests
{
    public class FileTests
    {
        private readonly Fixture _fixture;
        const string testJson =
                " {" +
                "   \"menuitem\": [" +
                "      {\"value\": \"New\", \"onclick\": \"CreateNewDoc()\"}," +
                "      {\"value\": \"Open\", \"onclick\": \"OpenDoc()\"}," +
                "      {\"value\": \"Close\", \"onclick\": \"CloseDoc()\"}" +
                "   ]" +
                "}";

        private const string defaultDealer = "defaultDealer";


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
            var fileContent = ReadFileAsync(GetBasePath() + fileName);
            var sut = new JsonParser();

            // Act
            var parcingResult = sut.Parse(fileContent, fileName, defaultDealer);

            // Assert
            parcingResult.Should().NotBeEmpty();
            parcingResult.Should().HaveCount(resultCarsCount);
            parcingResult.Should().BeOfType<List<CarActual>>();
        }

        [InlineData("Correct/CorrectCsv1_v1.csv", 1)]
        [InlineData("Correct/CorrectCsv2_v2.csv", 2)]
        [Theory]
        public void Parse_correct_csv_file(string fileName, int resultCarsCount)
        {
            // Arrange
            var fileContent = ReadFileAsync(GetBasePath() + fileName);
            var sut = new CsvParser();

            // Act
            var parcingResult = sut.Parse(fileContent, fileName, defaultDealer);

            // Assert
            parcingResult.Should().NotBeEmpty();
            parcingResult.Should().HaveCount(resultCarsCount);
            parcingResult.Should().BeOfType<List<CarActual>>();
        }

        [InlineData("Correct/CorrectYaml1.yaml", 1)]
        [Theory]
        public void Parse_correct_yaml_file(string fileName, int resultCarsCount)
        {
            // Arrange
            var fileContent = ReadFileAsync(GetBasePath() + fileName);
            var sut = new YamlParser();

            // Act
            var parcingResult = sut.Parse(fileContent, fileName, defaultDealer);

            // Assert
            parcingResult.Should().NotBeEmpty();
            parcingResult.Should().HaveCount(resultCarsCount);
            parcingResult.Should().BeOfType<List<CarActual>>();
        }

        [InlineData("Correct/CorrectXml1.xml", 1)]
        [Theory]
        public void Parse_correct_xml_file(string fileName, int resultCarsCount)
        {
            // Arrange
            var fileContent = ReadFileAsync(GetBasePath() + fileName);
            var sut = new XmlParser();

            // Act
            var parcingResult = sut.Parse(fileContent, fileName, defaultDealer);

            // Assert
            parcingResult.Should().NotBeEmpty();
            parcingResult.Should().HaveCount(resultCarsCount);
            parcingResult.Should().BeOfType<List<CarActual>>();
        }



        [Fact]
        public void Try_parse_invalid_json_file()
        {
            // Arrange
            var fileName = "Invalid/InvalidJson1.json";
            var fileContent = ReadFileAsync(GetBasePath() + fileName);
            var sut = new JsonParser();

            // Assert
            Assert.Throws<InvalidFileException>(() => sut.Parse(fileContent, fileName, defaultDealer));
        }

        [Fact]
        public void Try_parse_invalid_xml_file()
        {
            // Arrange
            var fileName = "Invalid/InvalidXml1.xml";
            var fileContent = ReadFileAsync(GetBasePath() + fileName);
            var sut = new XmlParser();

            // Assert
            Assert.Throws<InvalidFileException>(() => sut.Parse(fileContent, fileName, defaultDealer));
        }

        [Fact]
        public void Try_parse_invalid_csv_file()
        {
            // Arrange
            var fileName = "Invalid/InvalidCsv1_v1.csv";
            var fileContent = ReadFileAsync(GetBasePath() + fileName);
            var sut = new XmlParser();

            // Assert
            Assert.Throws<InvalidFileException>(() => sut.Parse(fileContent, fileName, defaultDealer));
        }

        [Fact]
        public void Try_parse_invalid_yaml_file()
        {
            // Arrange
            var fileName = "Invalid/InvalidYaml1.yaml";
            var fileContent = ReadFileAsync(GetBasePath() + fileName);
            var sut = new XmlParser();

            // Assert
            Assert.Throws<InvalidFileException>(() => sut.Parse(fileContent, fileName, defaultDealer));
        }






        // Разобраться с проверкой слишком больших файлов
        //[InlineData("TooLargeJsonFile1.json")]
        //public void Too_large_json_file(string fileName) { }

        private static string GetBasePath()
        {
            return @"..\..\..\TestFiles\";
        }
        private static string ReadFileAsync(string path)
        {
            using var reader = new StreamReader(path);
            return reader.ReadToEnd();
        }
    }
}
