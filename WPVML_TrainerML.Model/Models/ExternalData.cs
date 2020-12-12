using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.ML.Data;

namespace WPVML_TrainerML.Model.Models
{
    public class ExternalData
    {
        [ColumnName("Hour"), LoadColumn(0)]
        public float Hour { get; set; }

        [ColumnName("Day"), LoadColumn(1)]
        public float Day { get; set; }

        [ColumnName("Temperature"), LoadColumn(3)]
        public float Temperature { get; set; }

        [ColumnName("WindSpeed"), LoadColumn(4)]
        public float WindSpeed { get; set; }

        [ColumnName("Rain"), LoadColumn(5)]
        public float Rain { get; set; }

        [ColumnName("Cloudiness"), LoadColumn(6)]
        public float Cloudiness { get; set; }

        public void Print()
        {
            Console.WriteLine("Model of external data...\n\n");
            Console.WriteLine($"Day: {this.Day}");
            Console.WriteLine($"Hour: {this.Hour}");
            Console.WriteLine($"Temperature: {this.Temperature}");
            Console.WriteLine($"Wind speed: {this.WindSpeed}");
            Console.WriteLine($"Rain: {this.Rain}");
            Console.WriteLine($"Cloudiness: {this.Cloudiness}");
        }
    }

    public class ExternalDataOutput
    {
        [ColumnName("PredictedLabel")]
        public uint PredictedClusterId { get; set; }

        [ColumnName("Score")]
        public float[] Distances { get; set; }
    }
}
