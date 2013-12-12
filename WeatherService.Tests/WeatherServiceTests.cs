using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using WeatherService.Services;

namespace WeatherService.Tests
{
    [TestFixture]
    public class WeatherServiceTests
    {
        //[Test]
        //public void TestWrongCityNameResult()
        //{
        //    var openWeatherService = new OpenWeatherService();

        //    var result = openWeatherService.GetWeatherInfo("SomeWrongCityName");

        //    Assert.AreEqual(0, result.Count);
        //}

        //[Test]
        //public void TestRealCityNameResult()
        //{
        //    var openWeatherService = new OpenWeatherService();

        //    var result = openWeatherService.GetWeatherInfo("Chelyabinsk");

        //    Assert.AreNotEqual(0, result.Count);
        //}

        //[Test]
        //public void TestRealCityNameResultWithSpaces()
        //{
        //    var openWeatherService = new OpenWeatherService();

        //    var result = openWeatherService.GetWeatherInfo("San Francisco");

        //    Assert.AreNotEqual(0, result.Count);
        //}

        //[Test]
        //public void TestRealCityNameResultWithTrailingSpaces()
        //{
        //    var openWeatherService = new OpenWeatherService();

        //    var result = openWeatherService.GetWeatherInfo("  San Francisco   ");

        //    Assert.AreNotEqual(0, result.Count);
        //}
    }
}
