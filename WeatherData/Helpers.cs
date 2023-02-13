using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using WeatherData.Data;
using WeatherData.Models;

namespace WeatherData
{
    internal class Helpers
    {
        internal static List<double> GetTemperatureForSelectedMonth(DateTime date, List<SensorDataTime> sensorData)
        {
            List<double> avgTemperaturePerMonth = new()
            {
                0.0,
                0.0
            };
            List<DateTime> dayList = GetDatesPerMonthFromSensorData(date.Month, sensorData);

            for (int i = 0; i < dayList.Count; i++)
            {
                List<double> avgTemperaturePerDay = GetTemperatureForSelectedDay(dayList[i], sensorData);

                avgTemperaturePerMonth[0] += (avgTemperaturePerDay[0]);
                avgTemperaturePerMonth[1] += (avgTemperaturePerDay[1]);
            }
            avgTemperaturePerMonth[0] = avgTemperaturePerMonth[0] / dayList.Count;
            avgTemperaturePerMonth[1] = avgTemperaturePerMonth[1] / dayList.Count;
            return avgTemperaturePerMonth;
        }

        internal static List<SensorDataTime> GetAvgUnitPerMonthList(List<SensorDataTime> sensorData)
        {
            List<SensorDataTime> result = new();

            for (var i = 6; i <= 12; i++)
            {
                List<SensorDataTime> AvgUnitDataList = GetUnitForSelectedMonth(new DateTime(2016, i, 1), sensorData);

                double tempTemperature = 0;
                double tempHumidity = 0;
                double tempMold = 0;
                foreach (SensorDataTime unit in AvgUnitDataList)
                {
                    tempTemperature += double.Parse(unit.Temp);
                    tempHumidity += double.Parse(unit.Humidity);
                    tempMold += double.Parse(unit.MoldRisk);
                }

                result.Add(new SensorDataTime
                {
                    Date = AvgUnitDataList[0].Date,
                    Temp = (tempTemperature / (double)AvgUnitDataList.Count).ToString("0.00"),
                    Humidity = (tempHumidity / (double)AvgUnitDataList.Count).ToString("0"),
                    MoldRisk = (tempMold / (double)AvgUnitDataList.Count).ToString("0"),
                    Location = AvgUnitDataList[0].Location
                });
            }
            return result;
        }

        internal static List<SensorDataTime> GetAvgUnitPerDayList(List<SensorDataTime> sensorData)
        {
            List<SensorDataTime> result = new();

            for (var i = 6; i <= 12; i++)
            {
                List<SensorDataTime> AvgUnitDataList = GetUnitForSelectedMonth(new DateTime(2016, i, 1), sensorData);
                foreach (SensorDataTime unit in AvgUnitDataList) result.Add(unit);
            }
            return result;
        }

        private static List<double> GetHumidityForSelectedMonth(DateTime date, List<SensorDataTime> sensorData)
        {
            List<double> avgHumidityPerMonth = new()
            {
                0.0,
                0.0
            };
            List<DateTime> dayList = GetDatesPerMonthFromSensorData(date.Month, sensorData);

            for (int i = 0; i < dayList.Count; i++)
            {
                List<double> avgHumidityPerDay = GetHumidityForSelectedDay(dayList[i], sensorData);

                avgHumidityPerMonth[0] += (avgHumidityPerDay[0]);
                avgHumidityPerMonth[1] += (avgHumidityPerDay[1]);
            }
            avgHumidityPerMonth[0] = avgHumidityPerMonth[0] / dayList.Count;
            avgHumidityPerMonth[1] = avgHumidityPerMonth[1] / dayList.Count;
            return avgHumidityPerMonth;
        }

        internal static List<string> GetHumidityForAllMonths(List<SensorDataTime> sensorData)
        {
            List<string> avgHumidityPerMonthList = new();
            for (var i = 6; i <= 12; i++)
            {
                List<double> avgHumidityPerMonth = GetHumidityForSelectedMonth(new DateTime(2016, i, 1), sensorData);
                avgHumidityPerMonthList.Add(DateTimeFormatInfo.CurrentInfo.GetMonthName(i).PadRight(10) + ":   Inside: " + avgHumidityPerMonth[0].ToString("0") + " %   Outside: " + avgHumidityPerMonth[1].ToString("0") + " %");
            }
            return avgHumidityPerMonthList;
        }

        internal static List<string> FormatDates(List<DateTime> dates)
        {
            List<string> formattedDates = new();
            for (int i = 0; i < dates.Count; i++)
                formattedDates.Add(dates[i].DayOfWeek.ToString().PadRight(10) + ": " + dates[i].ToString("yyyy/MM/dd"));
            return formattedDates;
        }

        private static List<double> GetDivideDataTempPerLocation(List<SensorDataTime> insideData, List<SensorDataTime> outsideData)
        {
            return GetAverageTemperature(insideData, outsideData);
        }

        private static List<double> GetDivideDataHumidityPerLocation(List<SensorDataTime> insideData, List<SensorDataTime> outsideData)
        {
            return GetAverageHumidity(insideData, outsideData);
        }

        private static List<double> GetAverageTemperature(List<SensorDataTime> insideData, List<SensorDataTime> outsideData)
        {

            List<double> temps = new()
            {
                GetAverageTemperatureFromModel(18, 30, insideData),
                GetAverageTemperatureFromModel(-30, 36, outsideData)
            };
            return temps;
        }

        private static List<double> GetAverageHumidity(List<SensorDataTime> insideData, List<SensorDataTime> outsideData)
        {

            List<double> temps = new()
            {
                GetAverageHumidityFromModel(0, 100, insideData),
                GetAverageHumidityFromModel(0, 100, outsideData)
            };
            return temps;
        }

        private static double GetAverageTemperatureFromModel(double minTemp, double maxTemp, List<SensorDataTime> listData)
        {
            double temp = 0.0;
            foreach (SensorDataTime data in listData)
            {
                double temporaryTemp = double.Parse(data.Temp, System.Globalization.CultureInfo.InvariantCulture);
                if (temporaryTemp > minTemp && temporaryTemp < maxTemp)
                    temp += temporaryTemp;
            }
            return temp / (double)listData.Count;
        }

        private static double GetAverageHumidityFromModel(double minHumidity, double maxHumidity, List<SensorDataTime> listData)
        {
            double humidity = 0.0;
            foreach (SensorDataTime data in listData)
            {
                double temporaryHumidity = double.Parse(data.Humidity, System.Globalization.CultureInfo.InvariantCulture);
                if (temporaryHumidity > minHumidity && temporaryHumidity < maxHumidity)
                    humidity += temporaryHumidity;
            }
            return humidity / (double)listData.Count;
        }

        internal static List<DateTime> GetDatesPerMonthFromSensorData(int month, List<SensorDataTime> sensorData, int year = 2016)
        {
            List<DateTime> dates = new();
            foreach (var data in sensorData.Select(x => x.Date).Distinct())
                if (data.Date.Year == year && data.Date.Month == month)
                    dates.Add(data.Date);
            return dates;
        }

        private static List<SensorDataTime> GetSelectedDateData(DateTime dateDay, List<SensorDataTime> sensorData)
        {
            List<SensorDataTime> matches = new();
            foreach (var data in sensorData)
                if (data.Date.Year == dateDay.Year && data.Date.Month == dateDay.Month && data.Date.Day == dateDay.Day)
                    matches.Add(data);
            return matches;
        }

        internal static List<double> GetTemperatureForSelectedDay(DateTime dateDay, List<SensorDataTime> sensorData)
        {
            return GetDivideDataTempPerLocation(DivideDataPerLocation(GetSelectedDateData(dateDay, sensorData), "Inside"), DivideDataPerLocation(GetSelectedDateData(dateDay, sensorData), "Outside"));
        }

        private static List<double> GetHumidityForSelectedDay(DateTime dateDay, List<SensorDataTime> sensorData)
        {
            return GetDivideDataHumidityPerLocation(DivideDataPerLocation(GetSelectedDateData(dateDay, sensorData), "Inside"), DivideDataPerLocation(GetSelectedDateData(dateDay, sensorData), "Outside"));
        }

        internal static List<DateTime> GetMeteorological(List<SensorDataTime> sensorData, int minValue)
        {
            List<double> outsideDataTemp = new();
            List<DateTime> outsideDataDate = new();
            foreach (DateTime date in sensorData.Select(x => x.Date).Distinct())
            {
                List<double> DataTemp = GetTemperatureForSelectedDay(date, sensorData);
                outsideDataDate.Add(date);

                outsideDataTemp.Add(DataTemp[1]);
            }

            List<DateTime> meteroDays = new();
            double temproraryAverage = 5.0;

            for (int i = 0; i < outsideDataTemp.Count - 5; i++)
            {
                int counting = i;

                List<double> doubles = new();
                while (true)
                {
                    if (counting == i + 5)
                        break;

                    if (outsideDataTemp[counting] <= minValue)
                        doubles.Add(outsideDataTemp[counting]);

                    if (outsideDataDate[counting].Date.AddDays(1) != outsideDataDate[counting + 1].Date && counting != i + 4) 
                        break;

                    counting++;
                }

                if (doubles.Count == 5)
                {
                    if (meteroDays.Count == 0)
                    {
                        if (doubles.Average() <= minValue)
                        {
                            meteroDays.Add(outsideDataDate[counting - 5]);
                            meteroDays.Add(outsideDataDate[counting]);
                        }
                    }

                    else if (temproraryAverage > doubles.Average())
                    {
                        temproraryAverage = doubles.Average();
                        meteroDays[1] = outsideDataDate[counting - 5];
                    }
                }
            }
            return meteroDays;
        }

        internal static List<string> ConvertMeteoroligicalToStringList(List<DateTime> dateTimes)
        {
            List<string> result = new()
            {
                "Meteorologist Autumn Date: " + dateTimes[0].ToString("yyyy.MM.dd"),
                "Meteorologist Winter Date: " + dateTimes[1].ToString("yyyy.MM.dd")
            };
            return result;
        }

        private static List<SensorDataTime> GetUnitForSelectedMonth(DateTime date, List<SensorDataTime> sensorData)
        {
            List<DateTime> dayList = GetDatesPerMonthFromSensorData(date.Month, sensorData);
            List<SensorDataTime> unitAvgPerDay = new();
            for (int i = 0; i < dayList.Count; i++)
                unitAvgPerDay.Add(GetUnitAvgForSelectedDay(dayList[i], sensorData));
            return unitAvgPerDay;
        }

        private static SensorDataTime GetUnitAvgForSelectedDay(DateTime dateDay, List<SensorDataTime> sensorData)
        {
            List<SensorDataTime> matches = GetSelectedDateData(dateDay, sensorData);

            double temperature = 0.0;
            double humidity = 0.0;
            double moldRisk = 0.0;
            foreach (SensorDataTime data in matches)
            {

                double temporaryTemp = double.Parse(data.Temp, System.Globalization.CultureInfo.InvariantCulture);
                if (temporaryTemp > -30 && temporaryTemp < 40)
                    temperature += temporaryTemp;

                double temporaryHumidity = double.Parse(data.Humidity, System.Globalization.CultureInfo.InvariantCulture);
                if (temporaryHumidity > 0 && temporaryHumidity < 100)
                    humidity += temporaryHumidity;

            }
            temperature /= (double)matches.Count;
            humidity /= (double)matches.Count;
            moldRisk /= (double)matches.Count;

            SensorDataTime dataDay = new()
            {
                Date = dateDay,
                Temp = temperature.ToString("0.00"),
                Humidity = humidity.ToString("0"),
                MoldRisk = moldRisk.ToString("0"),
                Location = sensorData[0].Location,
            };

            return dataDay;
        }

        internal static List<SensorDataTime> DivideDataPerLocation(List<SensorDataTime> sensorData, string location)
        {
            List<SensorDataTime> locationData = new();
            foreach (SensorDataTime data in sensorData)
                if (data.Location == (location))
                    locationData.Add(data);
            return locationData;
        }

        internal static List<SensorDataTime> GetSortedListPerTemp(List<SensorDataTime> dataAvgPerDay)
        {
            return dataAvgPerDay
                .OrderByDescending(g => double.Parse(g.Temp, System.Globalization.CultureInfo.InvariantCulture))
                .Select(g => g).Take(5).ToList();
        }

        internal static List<SensorDataTime> GetSortedListPerTempReverse(List<SensorDataTime> dataAvgPerDay)
        {
            return dataAvgPerDay
                .OrderBy(g => double.Parse(g.Temp, System.Globalization.CultureInfo.InvariantCulture))
                .Select(g => g).Take(5).ToList();
        }

        internal static List<SensorDataTime> GetSortedListPerHumidity(List<SensorDataTime> dataAvgPerDay)
        {
            return dataAvgPerDay
                .OrderByDescending(g => double.Parse(g.Humidity, System.Globalization.CultureInfo.InvariantCulture))
                .Select(g => g).Take(5).ToList();
        }

        internal static List<SensorDataTime> GetSortedListPerHumidityReverse(List<SensorDataTime> dataAvgPerDay)
        {
            return dataAvgPerDay
                .OrderBy(g => double.Parse(g.Humidity, System.Globalization.CultureInfo.InvariantCulture))
                .Select(g => g).Take(5).ToList();
        }

        internal static List<SensorDataTime> GetSortedListPerMold(List<SensorDataTime> dataAvgPerDay)
        {
            return dataAvgPerDay
                .OrderByDescending(g => double.Parse(g.MoldRisk, System.Globalization.CultureInfo.InvariantCulture))
                .Select(g => g).ToList();
        }

        internal static List<string> ConvertModelListToStringListWithMolding(List<SensorDataTime> modelList)
        {
            List<string> unitList = new();
            for (int i = 0; i < modelList.Count; i++)
                unitList.Add(modelList[i].Date.ToString("dd-MMMM-yyyy").PadRight(20) + modelList[i].Location.PadRight(10) + "Temperature: " + modelList[i].Temp.PadRight(9) + "Humidity: " + modelList[i].Humidity.PadRight(2) + " %   Mold Risk: " + modelList[i].MoldRisk + " %");
            return unitList;
        }

        internal static List<string> ConvertModelListToStringList(List<SensorDataTime> modelList)
        {
            List<string> unitList = new();
            for (int i = 0; i < modelList.Count; i++)
                unitList.Add(modelList[i].Date.ToString("dd-MMMM-yyyy").PadRight(20) + modelList[i].Location.PadRight(10) + "Temperature: " + modelList[i].Temp.PadRight(9) + "Humidity: " + modelList[i].Humidity.PadRight(2) + " %");
            return unitList;
        }

        internal static List<string> ConvertModelListToStringListWithMoldingPerYear(List<SensorDataTime> modelList)
        {
            List<string> unitList = new();
            for (int i = 0; i < modelList.Count; i++)
                unitList.Add(DateTimeFormatInfo.CurrentInfo.GetMonthName(i+6).PadRight(10) + modelList[i].Location.PadRight(10) + "Temperature: " + modelList[i].Temp.PadRight(9) + "Humidity: " + modelList[i].Humidity.PadRight(2) + " %   Mold Risk: " + modelList[i].MoldRisk + " %");
            return unitList;
        }

        internal static string ConvertDoubleToStringListWithDate(List<double> avgTemperature)
        {
            return "  Inside: " + avgTemperature[0].ToString("0.00") + "  Outside: " + avgTemperature[1].ToString("0.00");
        }
    }
}
