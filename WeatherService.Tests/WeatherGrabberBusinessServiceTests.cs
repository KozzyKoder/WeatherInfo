using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessLayer.BusinessServices;
using Castle.MicroKernel.Registration;
using Common;
using DataAccess.Entities;
using DataAccess.Repository;
using Moq;
using NUnit.Framework;
using WeatherService.ServiceAggregator;

namespace WeatherService.Tests
{
    [TestFixture]
    public class WeatherGrabberBusinessServiceTests
    {
        private Mock<IRepository<WeatherInfo>> _weatherInfoRepositoryMock;
        private Mock<IServiceAggregator> _serviceAggregatorMock;
        private string ChelyabinskCityName = "Chelyabinsk";

        public IWeatherGrabberBusinessService GetSut()
        {
            return new WeatherGrabberBusinessService();
        }

        [SetUp]
        public void Setup()
        {
            _weatherInfoRepositoryMock = new Mock<IRepository<WeatherInfo>>();
            _serviceAggregatorMock = new Mock<IServiceAggregator>();

            Ioc.Container.Register(
                Castle.MicroKernel.Registration.Component.For<IRepository<WeatherInfo>>()
                    .Instance(_weatherInfoRepositoryMock.Object)
                    .LifestyleSingleton());

            Ioc.Container.Register(
                Castle.MicroKernel.Registration.Component.For<IServiceAggregator>()
                    .Instance(_serviceAggregatorMock.Object)
                    .LifestyleSingleton());
        }

        [Test]
        public void TestIfNoCityNameProvidedReturnsEmptyList()
        {
            var sut = GetSut();

            var result = sut.GrabWeatherInfos();

            Assert.NotNull(result);
            Assert.AreEqual(0, result.Count());
        }

        [Test]
        public void TestIfOneCityNameProvidedAndThisCityExistsInDatabaseReturnsListWithOneElement()
        {
            var sut = GetSut();

            _weatherInfoRepositoryMock
                .Setup(p => p.Get(It.IsAny<Func<WeatherInfo, bool>>()))
                .Returns(new WeatherInfo()
                {
                    CityName = ChelyabinskCityName
                });

            var result = sut.GrabWeatherInfos(ChelyabinskCityName).ToList();

            Assert.NotNull(result);
            Assert.AreEqual(1, result.Count());
            Assert.AreEqual(ChelyabinskCityName, result.First().CityName);
        }
    }
}
