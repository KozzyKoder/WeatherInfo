using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessLayer.BusinessServices;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Common;
using DataAccess.Entities;
using DataAccess.Repository;
using Moq;
using NUnit.Framework;
using WeatherService.ServiceAggregator;
using WeatherService.ServiceParameters;

namespace WeatherService.Tests
{
    [TestFixture]
    public class WeatherGrabberBusinessServiceTests
    {
        private Mock<IRepository<WeatherInfo>> _weatherInfoRepositoryMock;
        private Mock<IServiceAggregator<WeatherInfo, WeatherServiceParameters>> _serviceAggregatorMock;
        private Mock<IDateTimeProvider> _dateTimeProviderMock;
        private string ChelyabinskCityName = "Chelyabinsk";
        private DateTime _todayDate = new DateTime(2012, 10, 14, 12, 0, 0);

        public IWeatherGrabberBusinessService GetSut()
        {
            return new WeatherGrabberBusinessService();
        }

        [SetUp]
        public void FixtureSetup()
        {
            Ioc.Container = new WindsorContainer();
            
            _weatherInfoRepositoryMock = new Mock<IRepository<WeatherInfo>>();
            _serviceAggregatorMock = new Mock<IServiceAggregator<WeatherInfo, WeatherServiceParameters>>();
            _dateTimeProviderMock = new Mock<IDateTimeProvider>();
            _dateTimeProviderMock.Setup(p => p.UtcNow()).Returns(_todayDate);

            Ioc.Container.Register(
                Castle.MicroKernel.Registration.Component.For<IRepository<WeatherInfo>>()
                    .Instance(_weatherInfoRepositoryMock.Object)
                    .LifestyleSingleton());

            Ioc.Container.Register(
                Castle.MicroKernel.Registration.Component.For<IServiceAggregator<WeatherInfo, WeatherServiceParameters>>()
                    .Instance(_serviceAggregatorMock.Object)
                    .LifestyleSingleton());

            Ioc.Container.Register(
                Castle.MicroKernel.Registration.Component.For<IDateTimeProvider>()
                .Instance(_dateTimeProviderMock.Object)
                .LifestyleSingleton());
        }

        [Test]
        public void TestIfNoCityNameProvidedReturnsEmptyList()
        {
            var sut = GetSut();

            var result = sut.GrabWeatherInfos();

            Assert.NotNull(result);
            Assert.AreEqual(0, result.Count());
            _weatherInfoRepositoryMock.Verify(p => p.Save(It.IsAny<WeatherInfo>()), Moq.Times.Never);
            _weatherInfoRepositoryMock.Verify(p => p.Update(It.IsAny<WeatherInfo>()), Moq.Times.Never);
        }

        [Test]
        public void TestIfOneCityNameProvidedAndThisCityExistsInDatabaseReturnsListWithOneElement()
        {
            var sut = GetSut();

            _weatherInfoRepositoryMock
                .Setup(p => p.Get(It.IsAny<Func<WeatherInfo, bool>>()))
                .Returns(new WeatherInfo()
                {
                    CityName = ChelyabinskCityName,
                    LastUpdated = _todayDate
                });

            var result = sut.GrabWeatherInfos(ChelyabinskCityName).ToList();

            Assert.NotNull(result);
            Assert.AreEqual(1, result.Count());
            Assert.AreEqual(ChelyabinskCityName, result.First().CityName);
            _weatherInfoRepositoryMock.Verify(p => p.Save(It.IsAny<WeatherInfo>()), Moq.Times.Never);
            _weatherInfoRepositoryMock.Verify(p => p.Update(It.IsAny<WeatherInfo>()), Moq.Times.Never);
        }

        [Test]
        public void TestIfOneCityNameProvidedAndThisCityNotExistsInDatabaseReturnsListWithOneElementFromService()
        {
            var sut = GetSut();

            _weatherInfoRepositoryMock
                .Setup(p => p.Get(It.IsAny<Func<WeatherInfo, bool>>()))
                .Returns((WeatherInfo)null);

            _serviceAggregatorMock
                .Setup(p => p.AggregateServicesInfo(It.IsAny<WeatherServiceParameters>()))
                .Returns(new WeatherInfo()
                {
                    CityName = ChelyabinskCityName,
                    LastUpdated = _todayDate
                });

            var result = sut.GrabWeatherInfos(ChelyabinskCityName).ToList();

            Assert.NotNull(result);
            Assert.AreEqual(1, result.Count());
            Assert.AreEqual(ChelyabinskCityName, result.First().CityName);
            _weatherInfoRepositoryMock.Verify(p => p.Save(It.IsAny<WeatherInfo>()), Moq.Times.Once);
            _weatherInfoRepositoryMock.Verify(p => p.Update(It.IsAny<WeatherInfo>()), Moq.Times.Never);
        }

        [Test]
        public void TestIfOneCityNameProvidedAndThisCityExistsInDatabaseButTimeExpiredReturnsListWithOneElementFromService()
        {
            var sut = GetSut();

            _weatherInfoRepositoryMock
                .Setup(p => p.Get(It.IsAny<Func<WeatherInfo, bool>>()))
                .Returns(new WeatherInfo()
                {
                    CityName = ChelyabinskCityName,
                    LastUpdated = _todayDate.AddHours(-10)
                });

            _serviceAggregatorMock
                .Setup(p => p.AggregateServicesInfo(It.IsAny<WeatherServiceParameters>()))
                .Returns(new WeatherInfo()
                {
                    CityName = ChelyabinskCityName,
                    LastUpdated = _todayDate
                });

            var result = sut.GrabWeatherInfos(ChelyabinskCityName).ToList();

            Assert.NotNull(result);
            Assert.AreEqual(1, result.Count());
            Assert.AreEqual(ChelyabinskCityName, result.First().CityName);
            _weatherInfoRepositoryMock.Verify(p => p.Save(It.IsAny<WeatherInfo>()), Moq.Times.Never);
            _weatherInfoRepositoryMock.Verify(p => p.Update(It.IsAny<WeatherInfo>()), Moq.Times.Once);
        }

        [Test]
        public void TestIfOneCityNameProvidedAndThisCityExistsInDatabaseButTimeNotExpiredReturnsListWithOneElementFromService()
        {
            var sut = GetSut();

            _weatherInfoRepositoryMock
                .Setup(p => p.Get(It.IsAny<Func<WeatherInfo, bool>>()))
                .Returns(new WeatherInfo()
                {
                    CityName = ChelyabinskCityName,
                    LastUpdated = _todayDate.AddHours(-3)
                });

            _serviceAggregatorMock
                .Setup(p => p.AggregateServicesInfo(It.IsAny<WeatherServiceParameters>()))
                .Returns(new WeatherInfo()
                {
                    CityName = ChelyabinskCityName,
                    LastUpdated = _todayDate
                });

            var result = sut.GrabWeatherInfos(ChelyabinskCityName).ToList();

            Assert.NotNull(result);
            Assert.AreEqual(1, result.Count());
            Assert.AreEqual(ChelyabinskCityName, result.First().CityName);
            _weatherInfoRepositoryMock.Verify(p => p.Save(It.IsAny<WeatherInfo>()), Moq.Times.Never);
            _weatherInfoRepositoryMock.Verify(p => p.Update(It.IsAny<WeatherInfo>()), Moq.Times.Never);
        }
    }
}
