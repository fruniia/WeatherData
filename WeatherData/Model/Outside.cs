﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherData.Model
{
    internal class Outside : Location
    {
        public DateTime Date { get; set; }
        public string Location { get; set; }
        public double Temperature { get; set; }
        public double Humidity { get; set; }
    }
}
