using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Subscriber.Data;
using Subscriber.Models;
using Subscriber.Services;
using Xunit;

namespace Subscriber.Tests
{
    public class SubscriberTests
    {
        private readonly SubscriberServices _service;
        public SubscriberTests()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new WebApi.Mappings.AutoMapper());
            });
            var mapper = config.CreateMapper();
            var options = new DbContextOptionsBuilder<WeightWatchers>()
                .UseSqlServer("Server =.\\sqlexpress; Database = WeightWatchers; Trusted_Connection = True;")
                .Options;

            var context = new WeightWatchers(options);
            var subeRepo = new SubscriberRepository(mapper, context);
            _service = new SubscriberServices(subeRepo);
        }
        [Fact]
        public void checkSubscriberEmail_DoesNotExists_ReturnsZero()
        {
            //Arrange
            var email = "renamarkiewitz@gmail.com";
            var password = "rena@weightwatchers";

            //Act
            var result = _service.checkSubscriberEmail(email, password);

            //Assert
            Assert.Equal(0, result);
        }

       
    }
}
