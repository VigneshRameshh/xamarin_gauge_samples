﻿using System;
using System.ComponentModel;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace QiblaLocater
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        double current_latitude = 25.2618616;
        double current_longitude = 55.3254198;
        double QiblaLatitude = 21.4224779;
        double QiblaLongitude = 39.8251832;
        SensorSpeed speed = SensorSpeed.UI;
        public MainPage()
        {
            InitializeComponent();
            GetLocation();
            PointToQibla();
            Compass.ReadingChanged += Compass_ReadingChanged;
            if (Device.RuntimePlatform != Device.UWP)
            {
                if (!Compass.IsMonitoring) Compass.Start(speed);
            }
        }

        void Compass_ReadingChanged(object sender, CompassChangedEventArgs e)
        {
            circularGauge.RotateTo(360 - e.Reading.HeadingMagneticNorth);
            PointToQibla();
        }

        private void Scale_LabelCreated(object sender, Syncfusion.SfGauge.XForms.LabelCreatedEventArgs args)
        {
            switch ((string)args.LabelContent)
            {
                case "0":
                    args.LabelContent = "N";
                    break;
                case "45":
                    args.LabelContent = "NE";
                    break;
                case "90":
                    args.LabelContent = "E";
                    break;
                case "135":
                    args.LabelContent = "SE";
                    break;
                case "180":
                    args.LabelContent = "S";
                    break;
                case "225":
                    args.LabelContent = "SW";
                    break;
                case "270":
                    args.LabelContent = "W";
                    break;
                case "315":
                    args.LabelContent = "NW";
                    break;
                case "360":
                    args.LabelContent = "N";
                    break;
            }
        }

        private async void GetLocation()
        {
            var request = new GeolocationRequest(GeolocationAccuracy.Medium);
            var location = await Geolocation.GetLocationAsync(request);
            current_latitude = location.Latitude;
            current_longitude = location.Longitude;
        }

        private double Mod(double a, double b)
        {
            return a - b * Math.Floor(a / b);
        }

        void PointToQibla()
        {
            double latt_from_radians = current_latitude * Math.PI / 180;
            double long_from_radians = current_longitude * Math.PI / 180;
            double latt_to_radians = QiblaLatitude * Math.PI / 180;
            double lang_to_radians = QiblaLongitude * Math.PI / 180;
            double bearing = Math.Atan2(Math.Sin(lang_to_radians - long_from_radians) * Math.Cos(latt_to_radians), (Math.Cos(latt_from_radians) * Math.Sin(latt_to_radians)) - (Math.Sin(latt_from_radians) * Math.Cos(latt_to_radians) * Math.Cos(lang_to_radians - long_from_radians)));
            bearing = Mod(bearing, 2 * Math.PI);
            double bearing_degree = bearing * 180 / Math.PI;
            pointer1.Value = bearing_degree;
        }
    }
}
