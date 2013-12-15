using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessLayer.ServiceAggregator;
using BusinessLayer.Services;
using Common;
using DataAccess.Entities;
using Moq;
using NUnit.Framework;

namespace WeatherService.Tests
{
    [TestFixture]
    public class WeatherServiceAggregatorTests
    {
        private Mock<IWeatherService> _weatherService1Mock;
        private Mock<IWeatherService> _weatherService2Mock;
        private Mock<IDateTimeProvider> _datetimeProviderMock;

        private const string CityName = "Chelyabinsk";
        private const string CountryName1 = "Russia";
        private const string DescriptionText1 = "Description";
        private const string ElevationText1 = "460 ft";
        private const double Longitude1 = 60.0;
        private const double Latitude1 = 80.0;
        private const int PressureMb1 = 985;
        private const string RelativeHumidity1 = "95%";
        private const double TemperatureCelcius1 = -30.0;
        private const double VisibilityDistance1 = 10.0;
        private const int WindAngle1 = 120;
        private const string WindDirection1 = "NNW";
        private const double WindSpeedKph1 = 5.0;
        private const double WindSpeedMs1 = 3.0;

        private const string CountryName2 = "RU";
        private const string DescriptionText2 = "SomeText";
        private const string ElevationText2 = "560 ft";
        private readonly DateTime LastUpdated = new DateTime(2013, 12, 12, 12, 0, 0);
        private const double Longitude2 = 25.0;
        private const double Latitude2 = 40.0;
        private const int PressureMb2 = 905;
        private const string RelativeHumidity2 = "90%";
        private const double TemperatureCelcius2 = -20.0;
        private const double VisibilityDistance2 = 50.0;
        private const int WindAngle2 = 140;
        private const string WindDirection2 = "NW";
        private const double WindSpeedKph2 = 15.0;
        private const double WindSpeedMs2 = 13.0;

        private double delta = 0.01;

        private WeatherServiceAggregator GetSut()
        {
            return new WeatherServiceAggregator(new List<IWeatherService>()
            {
                _weatherService1Mock.Object,
                _weatherService2Mock.Object
            }, _datetimeProviderMock.Object);
        }

        private WeatherServiceAggregator GetEmptySut()
        {
            return new WeatherServiceAggregator(new List<IWeatherService>(), _datetimeProviderMock.Object);
        }

        [SetUp]
        public void SetUp()
        {
            _weatherService1Mock = new Mock<IWeatherService>();
            _weatherService2Mock = new Mock<IWeatherService>();
            _datetimeProviderMock = new Mock<IDateTimeProvider>();

            _datetimeProviderMock.Setup(p => p.UtcNow()).Returns(LastUpdated);
        }

        [Test]
        public void TestSecondServiceBeforeFirst()
        {
            _weatherService1Mock.Setup(p => p.GetWeatherInfo(CityName)).Returns(GetWeatherInfoForService1());
            _weatherService1Mock.Setup(p => p.Priority()).Returns(0);
            _weatherService2Mock.Setup(p => p.GetWeatherInfo(CityName)).Returns(GetWeatherInfoForService2());
            _weatherService2Mock.Setup(p => p.Priority()).Returns(1);

            var sut = GetSut();
            var aggregatedWeatherInfo = sut.Aggregate(CityName);

            Assert.AreEqual(CityName, aggregatedWeatherInfo.CityName);
            Assert.AreEqual(CountryName2, aggregatedWeatherInfo.Country);
            Assert.AreEqual(DescriptionText2, aggregatedWeatherInfo.Description);
            Assert.AreEqual(ElevationText2, aggregatedWeatherInfo.Elevation);
            Assert.AreEqual(LastUpdated, aggregatedWeatherInfo.LastUpdated);
            Assert.AreEqual(RelativeHumidity2, aggregatedWeatherInfo.RelativeHumidity);
            Assert.AreEqual(WindDirection2, aggregatedWeatherInfo.WindDirection);

            TestAverageValues(aggregatedWeatherInfo);
        }

        [Test]
        public void TestFirstServiceBeforeSecond()
        {
            _weatherService1Mock.Setup(p => p.GetWeatherInfo(CityName)).Returns(GetWeatherInfoForService1());
            _weatherService1Mock.Setup(p => p.Priority()).Returns(1);
            _weatherService2Mock.Setup(p => p.GetWeatherInfo(CityName)).Returns(GetWeatherInfoForService2());
            _weatherService2Mock.Setup(p => p.Priority()).Returns(0);

            var sut = GetSut();
            var aggregatedWeatherInfo = sut.Aggregate(CityName);

            Assert.AreEqual(CityName, aggregatedWeatherInfo.CityName);
            Assert.AreEqual(CountryName1, aggregatedWeatherInfo.Country);
            Assert.AreEqual(DescriptionText1, aggregatedWeatherInfo.Description);
            Assert.AreEqual(ElevationText1, aggregatedWeatherInfo.Elevation);
            Assert.AreEqual(LastUpdated, aggregatedWeatherInfo.LastUpdated);
            Assert.AreEqual(RelativeHumidity1, aggregatedWeatherInfo.RelativeHumidity);
            Assert.AreEqual(WindDirection1, aggregatedWeatherInfo.WindDirection);

            TestAverageValues(aggregatedWeatherInfo);
        }

        [Test]
        public void TestFirstServiceBeforeSecondButFirstServiceReturnsEmptyValues()
        {
            _weatherService1Mock.Setup(p => p.GetWeatherInfo(CityName)).Returns(new WeatherInfo());
            _weatherService1Mock.Setup(p => p.Priority()).Returns(1);
            _weatherService2Mock.Setup(p => p.GetWeatherInfo(CityName)).Returns(GetWeatherInfoForService2());
            _weatherService2Mock.Setup(p => p.Priority()).Returns(0);

            var sut = GetSut();
            var aggregatedWeatherInfo = sut.Aggregate(CityName);

            Assert.AreEqual(CityName, aggregatedWeatherInfo.CityName);
            Assert.AreEqual(CountryName2, aggregatedWeatherInfo.Country);
            Assert.AreEqual(DescriptionText2, aggregatedWeatherInfo.Description);
            Assert.AreEqual(ElevationText2, aggregatedWeatherInfo.Elevation);
            Assert.AreEqual(LastUpdated, aggregatedWeatherInfo.LastUpdated);
            Assert.AreEqual(RelativeHumidity2, aggregatedWeatherInfo.RelativeHumidity);
            Assert.AreEqual(WindDirection2, aggregatedWeatherInfo.WindDirection);

            Assert.AreEqual(Latitude2, aggregatedWeatherInfo.Latitude, delta);
            Assert.AreEqual(Longitude2, aggregatedWeatherInfo.Longitude, delta);
            Assert.AreEqual(PressureMb2, aggregatedWeatherInfo.PressureMb, delta);
            Assert.AreEqual(TemperatureCelcius2, aggregatedWeatherInfo.TemperatureCelcius, delta);
            Assert.AreEqual(VisibilityDistance2, aggregatedWeatherInfo.VisibilityDistance, delta);
            Assert.AreEqual(WindAngle2, aggregatedWeatherInfo.WindAngle, delta);
            Assert.AreEqual(WindSpeedKph2, aggregatedWeatherInfo.WindSpeedKph, delta);
            Assert.AreEqual(WindSpeedMs2, aggregatedWeatherInfo.WindSpeedMs, delta);
        }

        [Test]
        public void TestServicesReturnNull()
        {
            var sut = GetSut();
            var aggregatedWeatherInfo = sut.Aggregate(CityName);

            Assert.AreEqual(CityName, aggregatedWeatherInfo.CityName);
            Assert.AreEqual(null, aggregatedWeatherInfo.Country);
            Assert.AreEqual(null, aggregatedWeatherInfo.Description);
            Assert.AreEqual(null, aggregatedWeatherInfo.Elevation);
            Assert.AreEqual(LastUpdated, aggregatedWeatherInfo.LastUpdated);
            Assert.AreEqual(null, aggregatedWeatherInfo.RelativeHumidity);
            Assert.AreEqual(null, aggregatedWeatherInfo.WindDirection);
        }

        [Test]
        public void TestServicesNotFound()
        {
            var sut = GetEmptySut();
            var aggregatedWeatherInfo = sut.Aggregate(CityName);

            Assert.AreEqual(CityName, aggregatedWeatherInfo.CityName);
            Assert.AreEqual(null, aggregatedWeatherInfo.Country);
            Assert.AreEqual(null, aggregatedWeatherInfo.Description);
            Assert.AreEqual(null, aggregatedWeatherInfo.Elevation);
            Assert.AreEqual(LastUpdated, aggregatedWeatherInfo.LastUpdated);
            Assert.AreEqual(null, aggregatedWeatherInfo.RelativeHumidity);
            Assert.AreEqual(null, aggregatedWeatherInfo.WindDirection);
        }

        private void TestAverageValues(WeatherInfo aggregatedWeatherInfo)
        {
            Assert.AreEqual((Latitude1 + Latitude2)/2, aggregatedWeatherInfo.Latitude, delta);
            Assert.AreEqual((Longitude1 + Longitude2)/2, aggregatedWeatherInfo.Longitude, delta);
            Assert.AreEqual((PressureMb1 + PressureMb2)/2, aggregatedWeatherInfo.PressureMb, delta);
            Assert.AreEqual((TemperatureCelcius1 + TemperatureCelcius2)/2, aggregatedWeatherInfo.TemperatureCelcius, delta);
            Assert.AreEqual((VisibilityDistance1 + VisibilityDistance2)/2, aggregatedWeatherInfo.VisibilityDistance, delta);
            Assert.AreEqual((WindAngle1 + WindAngle2)/2, aggregatedWeatherInfo.WindAngle, delta);
            Assert.AreEqual((WindSpeedKph1 + WindSpeedKph2)/2, aggregatedWeatherInfo.WindSpeedKph, delta);
            Assert.AreEqual((WindSpeedMs1 + WindSpeedMs2)/2, aggregatedWeatherInfo.WindSpeedMs, delta);
        }

        private WeatherInfo GetWeatherInfoForService1()
        {
            return new WeatherInfo()
            {
                CityName = null,
                Country = CountryName1,
                Description = DescriptionText1,
                Elevation = ElevationText1,
                LastUpdated = DateTime.UtcNow,
                Latitude = Latitude1,
                Longitude = Longitude1,
                PressureMb = PressureMb1,
                RelativeHumidity = RelativeHumidity1,
                TemperatureCelcius = TemperatureCelcius1,
                VisibilityDistance = VisibilityDistance1,
                WindAngle = WindAngle1,
                WindDirection = WindDirection1,
                WindSpeedKph = WindSpeedKph1,
                WindSpeedMs = WindSpeedMs1,
            };
        }

        private WeatherInfo GetWeatherInfoForService2()
        {
            return new WeatherInfo()
            {
                CityName = null,
                Country = CountryName2,
                Description = DescriptionText2,
                Elevation = ElevationText2,
                LastUpdated = DateTime.UtcNow,
                Latitude = Latitude2,
                Longitude = Longitude2,
                PressureMb = PressureMb2,
                RelativeHumidity = RelativeHumidity2,
                TemperatureCelcius = TemperatureCelcius2,
                VisibilityDistance = VisibilityDistance2,
                WindAngle = WindAngle2,
                WindDirection = WindDirection2,
                WindSpeedKph = WindSpeedKph2,
                WindSpeedMs = WindSpeedMs2,
                Id = new Guid()
            };
        }
    }
}
