using ApiDPSystem.Entities;
using ApiDPSystem.Models.Parser;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.IO;
using Xunit;

namespace DPSystem.Tests
{
    public class FileTests
    {
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



        [InlineData("TestJson2.json", 5)]
        [InlineData("TestJson3.json", 2)]
        [Theory]
        public void Parse_json_file_with_available_data(string fileName, int resultCarsCount)
        {
            // Arrange
            var dealer = "defaultDealer";
            var fileContent = ReadFileAsync(GetBasePath() + fileName);
            var sut = new JsonParser();

            // Act
            var parcingResult = sut.Parse(fileContent, fileName, dealer);

            // Assert
            parcingResult.Should().NotBeEmpty();
            parcingResult.Should().HaveCount(resultCarsCount);
            parcingResult.Should().BeOfType<List<CarActual>>();
        }


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
