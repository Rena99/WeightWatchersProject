using AutoMapper;
using Measure.Data;
using Measure.Services;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using Xunit;

namespace Measure.Tests
{
    public class MeasureTests
    {
        private readonly MeasureService _service;
        public MeasureTests()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new WebApi.AutoMapper());
            });
            var mapper = config.CreateMapper();
            var measure = new Mock<DbSet<Data.Measure>>();
            var options = new DbContextOptionsBuilder<MeasureContext>()
                .UseSqlServer("Server =.\\sqlexpress; Database = MeasureDB; Trusted_Connection = True;")
                .Options;

            var context = new MeasureContext(options);
            var measureRepo = new MeasureRepository(context, mapper);
            _service = new MeasureService(measureRepo);
        }
        [Fact]
        public void PostMeasure_Exists_ReturnId()
        {
            //Arrange
            var measureModel = new MeasureModel()
            {
                CardId = 1,
                Weight = 45,
                Status = "in progress"
            };

            //Act
            var result= _service.PostMeasure(measureModel);

            //Assert
            Assert.Equal("1", result.ToString());

        }

        [Fact]
        public void PostMeasure_New_ReturnId()
        {
            //Arrange
            var measureModel = new MeasureModel()
            {
                CardId = 7,
                Weight = 45,
                Status = "in progress"
            };

            //Act
            var result = _service.PostMeasure(measureModel);

            //Assert
            Assert.NotEqual("1", result.ToString());
            Assert.NotEqual("16", result.ToString());
        }
    }
}
