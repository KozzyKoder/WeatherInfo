using System;

namespace BusinessLayer.Services.Wunderground
{
    public class WundergroundServiceModel : IServiceModel
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
