using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

using Microsoft.ML.Data;

namespace WPVML_TrainerML.Model
{
    public class MostImportantPage
    {
        [ColumnName("Hour"), LoadColumn(1)]
        public int Hour { get; set; }

        [ColumnName("Day"), LoadColumn(2)]
        public int Day { get; set; }

        [ColumnName("Latitude"), LoadColumn(3)]
        public double LocationLatitude { get; set; }

        [ColumnName("Longitude"), LoadColumn(4)]
        public double LocationLongitude { get; set; }

        [ColumnName("MostImportantPage"), LoadColumn(5)]
        public int NodeId { get; set; }

        [ColumnName("Temperature"), LoadColumn(6)]
        public double Temperature { get; set; }

        public void Print()
        {
            Console.WriteLine("Using model to make single prediction -- Comparing actual PageVisits with predicted PageVisits from sample data...\n\n");
            Console.WriteLine($"Day: {this.Day}");
            Console.WriteLine($"Hour: {this.Hour}");
            Console.WriteLine($"Location: {this.LocationLatitude}, {this.LocationLongitude}");
            Console.WriteLine($"Temperature: {this.Temperature}");
            Console.WriteLine($"MostImportantPage: {this.NodeId}");
        }
    }
}
