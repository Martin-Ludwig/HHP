using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microcharts;
using HHP.Models;
using Entry = Microcharts.Entry; // important (overriding Entry with Microcharts Entry definition)
using SkiaSharp;
using System.Drawing;
using Xamarin.Forms;



namespace HHP.Utils.Diagram
{
    /// <summary>
    /// The main Diagram class.
    /// Contains all methods for generating diagrams.
    /// </summary>
    public class Diagram
    {
        public static List<SKColor> Colors { get; set; }
        /// <summary>
        /// Gernerates colors out of an integer.
        /// </summary>
        public static List<SKColor> GenerateColors(int n)
        {
            List<SKColor> colors = new List<SKColor>();
            for (int i = 0; i < n; i++)
            {
                colors.Add(SKColor.FromHsl(360 * (i + 1) / n, 65, 50));
            }
            return colors;
        }
        public static DonutChart CreatePieChart(List<Tuple<String, Double>> data)
        { 
            List<SKColor> colors = GenerateColors(data.Count);
            List<Entry> entries = new List<Entry>();

            for (int i = 0; i < data.Count; i++)
            {
                double value = Math.Round(data[i].Item2, 2);
                entries.Add(new Entry((float)value)
                {
                    Color = colors[i],
                    Label = data[i].Item1,
                    ValueLabel = $"{value} €"
                });
            }

            float _labelTextSize = 24;
            String _backgroundColor = "FFFFFF";
            if (Device.RuntimePlatform == Device.UWP)
            {
                // windows
                _labelTextSize = 20;
                _backgroundColor = "FFFFFF";
            }
            else if (Device.RuntimePlatform == Device.Android)
            {
                //android
                _labelTextSize = 48;
                _backgroundColor = "FAFAFA";
            }

            return new DonutChart() {
                Entries = entries,
                HoleRadius = 0.6f,
                LabelTextSize = _labelTextSize,
                BackgroundColor = SKColor.Parse(_backgroundColor),
            };
        }
        /// <summary>
        /// Creates and returns a LineChart.
        /// </summary>
        public static LineChart YearChart(List<Tuple<String, Double>> data)
        {
            List<SKColor> colors = GenerateColors(data.Count);
            List<Entry> entries = new List<Entry>();

            for (int i = 0; i < data.Count; i++)
            {
                double value = Math.Round(data[i].Item2, 2);
                entries.Add(new Entry((float)value)
                {
                    Color = colors[i],
                    Label = data[i].Item1,
                    ValueLabel = $"{value} €"
                });
            }

            float _labelTextSize = 24;
            String _backgroundColor = "FFFFFF";

            if (Device.RuntimePlatform == Device.UWP)
            {
                //windows
                _labelTextSize = 20;
                _backgroundColor = "FFFFFF";
            }
            else if (Device.RuntimePlatform == Device.Android)
            {
                //android
                _labelTextSize = 40;
                _backgroundColor = "FAFAFA";
            }

            return new LineChart()
            {
                Entries = entries,
                LineMode = LineMode.Straight,
                LabelTextSize = _labelTextSize,
                BackgroundColor = SKColor.Parse(_backgroundColor),
            };
        }

    }
}
