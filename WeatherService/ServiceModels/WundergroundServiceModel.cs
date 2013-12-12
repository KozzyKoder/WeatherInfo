using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherService.ServiceModels
{
    public class WundergroundServiceModel
    {
        public WundergroundObservation CurrentObservation { get; set; }
    }

    public class WundergroundObservation
    {
        public Double TempC { get; set; }
        public string RelativeHumidity { get; set; }
        public int PressureMb { get; set; }
        public Double VisibilityKm { get; set; }
        public Double WindKph { get; set; }
        public int WindDegrees { get; set; }
        public string WindDir { get; set; }
        public WundergroundObservationLocation ObservationLocation { get; set; }
    }

    public class WundergroundObservationLocation
    {
        public string Elevation { get; set; }
    }
}
