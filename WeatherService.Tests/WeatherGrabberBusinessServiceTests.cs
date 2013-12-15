using System;
using System.Collections.Generic;
using System.Linq;
using BusinessLayer.BusinessServices;
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
        private Mock<IWeatherServiceAggregator> _serviceAggregatorMock;
        private Mock<IDateTimeProvider> _dateTimeProviderMock;
        private const string ChelyabinskCityName = "Chelyabinsk";
        private DateTime _todayDate = new DateTime(2012, 10, 14, 12, 0, 0);
        private readonly List<string> _citiesList = new List<string>(){ ChelyabinskCityName };

        public IWeatherGrabberBusinessService GetSut()
        {
            return new WeatherGrabberBusinessService(_weatherInfoRepositoryMock.Object,
                                                     _dateTimeProviderMock.Object,
                                                     _serviceAggregatorMock.Object);
        }

        [TestFixtureSetUp]
        public void FixtureSetup()
        {
            _serviceAggregatorMock = new Mock<IWeatherServiceAggregator>();
            _dateTimeProviderMock = new Mock<IDateTimeProvider>();
            _dateTimeProviderMock.Setup(p => p.UtcNow()).Returns(_todayDate);
        }

        [SetUp]
        public void SetUp()
        {
            _weatherInfoRepositoryMock = new Mock<IRepository<WeatherInfo>>();
        }

        [Test]
        public void TestIfNoCityNameProvidedReturnsEmptyList()
        {
            var sut = GetSut();

            var result = sut.GrabWeatherInfos(new List<string>());

            Assert.NotNull(result);
            Assert.AreEqual(0, result.Count());
            _weatherInfoRepositoryMock.Verify(p => p.Save(It.IsAny<WeatherInfo>()), Times.Never);
            _weatherInfoRepositoryMock.Verify(p => p.Update(It.IsAny<WeatherInfo>()), Times.Never);
        }

        [Test]
        public void TestIfOneCityNameProvidedAndThisCityExistsInDatabaseReturnsListWithOneElement()
        {
            var sut = GetSut();

            SetupWeatherInfoRepositoryMock(_todayDate);

            var result = sut.GrabWeatherInfos(_citiesList).ToList();

            Assert.NotNull(result);
            Assert.AreEqual(1, result.Count());
            Assert.AreEqual(ChelyabinskCityName, result.First().CityName);
            _weatherInfoRepositoryMock.Verify(p => p.Save(It.IsAny<WeatherInfo>()), Times.Never);
            _weatherInfoRepositoryMock.Verify(p => p.Update(It.IsAny<WeatherInfo>()), Times.Never);
        }

        [Test]
        public void TestIfOneCityNameProvidedAndThisCityNotExistsInDatabaseReturnsListWithOneElementFromService()
        {
            var sut = GetSut();

            _weatherInfoRepositoryMock
                .Setup(p => p.Get(It.IsAny<Func<WeatherInfo, bool>>()))
                .Returns((WeatherInfo)null);

            SetupServiceAggregatorMock();

            var result = sut.GrabWeatherInfos(_citiesList).ToList();

            Assert.NotNull(result);
            Assert.AreEqual(1, result.Count());
            Assert.AreEqual(ChelyabinskCityName, result.First().CityName);
            _weatherInfoRepositoryMock.Verify(p => p.Save(It.IsAny<WeatherInfo>()), Times.Once);
            _weatherInfoRepositoryMock.Verify(p => p.Update(It.IsAny<WeatherInfo>()), Times.Never);
        }

        [Test]
        public void TestIfOneCityNameProvidedAndThisCityExistsInDatabaseButTimeExpiredReturnsListWithOneElementFromService()
        {
            var sut = GetSut();

            SetupWeatherInfoRepositoryMock(_todayDate.AddHours(-10));

            SetupServiceAggregatorMock();

            var result = sut.GrabWeatherInfos(_citiesList).ToList();

            Assert.NotNull(result);
            Assert.AreEqual(1, result.Count());
            Assert.AreEqual(ChelyabinskCityName, result.First().CityName);
            _weatherInfoRepositoryMock.Verify(p => p.Save(It.IsAny<WeatherInfo>()), Times.Never);
            _weatherInfoRepositoryMock.Verify(p => p.Update(It.IsAny<WeatherInfo>()), Times.Once);
        }

        [Test]
        public void TestIfOneCityNameProvidedAndThisCityExistsInDatabaseButTimeNotExpiredReturnsListWithOneElementFromService()
        {
            var sut = GetSut();

            SetupWeatherInfoRepositoryMock(_todayDate.AddHours(-3));

            SetupServiceAggregatorMock();

            var result = sut.GrabWeatherInfos(_citiesList).ToList();

            Assert.NotNull(result);
            Assert.AreEqual(1, result.Count());
            Assert.AreEqual(ChelyabinskCityName, result.First().CityName);
            _weatherInfoRepositoryMock.Verify(p => p.Save(It.IsAny<WeatherInfo>()), Times.Never);
            _weatherInfoRepositoryMock.Verify(p => p.Update(It.IsAny<WeatherInfo>()), Times.Never);
        }

        private void SetupWeatherInfoRepositoryMock(DateTime lastUpdated)
        {
            _weatherInfoRepositoryMock
                .Setup(p => p.Get(It.IsAny<Func<WeatherInfo, bool>>()))
                .Returns(new WeatherInfo
                {
                    CityName = ChelyabinskCityName,
                    LastUpdated = lastUpdated
                });
        }

        private void SetupServiceAggregatorMock()
        {
            _serviceAggregatorMock
                .Setup(p => p.Aggregate(It.IsAny<string>()))
                .Returns(new WeatherInfo
                {
                    CityName = ChelyabinskCityName,
                    LastUpdated = _todayDate
                });
        }
    }
}
