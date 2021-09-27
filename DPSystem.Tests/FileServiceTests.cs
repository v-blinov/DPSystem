using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using ApiDPSystem.Data;
using ApiDPSystem.Entities;
using ApiDPSystem.Repository.Interfaces;
using ApiDPSystem.Services;
using ApiDPSystem.Services.Interfaces;
using AutoFixture;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace DPSystem.Tests
{
    public class FileServiceTest
    {
        private const string DefaultDealer = "DefaultDealer";

        private const string TestConnectionString = "Server=mssql,8083;Database=DPSystem.Tests;User=sa;Password=Qwerty123!;";
        private readonly DbContextOptions<Context> _testContextOptions = new DbContextOptionsBuilder<Context>().UseSqlServer(TestConnectionString).Options;

        public Fixture _fixture;
        
        private void CreateDatabase()
        {
            using var context = new Context(_testContextOptions);

            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
            
            _fixture = new Fixture();
        }

        [InlineData("Correct/DefaultJson.json")]
        [InlineData("Correct/DefaultXml.xml")]
        [InlineData("Correct/DefaultCsv_v1.csv")]
        [InlineData("Correct/DefaultYaml.yaml")]
        [Theory]
        public async Task File_parsing_method_test(string fileNameParam)
        {
            //Arrange
            var fileMock = GetIFormFile(fileNameParam);
            CreateDatabase();
        
            var dcsMock = new Mock<IDataCheckerService>();
            var crMock = new Mock<ICarRepository>();
            var sut = new FileService(dcsMock.Object, crMock.Object);
        
            //Act
            await sut.ProcessFileAsync(fileMock.Object, DefaultDealer);
        
            //Assert
            dcsMock.Verify(p => p.MarkSoldCars(It.IsAny<List<Car>>(), DefaultDealer), Times.Once);
            dcsMock.Verify(p => p.SetToDatabase(It.IsAny<List<Car>>()), Times.Once);
        }

        private static string GetBasePath() =>
            @"..\..\..\TestFiles\";
        private static string ReadFile(string path)
        {
            using var reader = new StreamReader(path);
            return reader.ReadToEnd();
        }
        private Mock<IFormFile> GetIFormFile(string name)
        {
            var fileMock = new Mock<IFormFile>();


            var fileName = name;
            var content = ReadFile(GetBasePath() + name);
            var ms = new MemoryStream();
            var writer = new StreamWriter(ms);
            writer.Write(content);
            writer.Flush();
            ms.Position = 0;
            fileMock.Setup(_ => _.OpenReadStream()).Returns(ms);
            fileMock.Setup(_ => _.FileName).Returns(fileName);
            fileMock.Setup(_ => _.Length).Returns(ms.Length);

            return fileMock;
        }
    }
}