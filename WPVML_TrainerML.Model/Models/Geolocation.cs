using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.ML.Data;

namespace WPVML_TrainerML.Model.Models
{
    public class Geolocation
    {
        [ColumnName("Laititude"), LoadColumn(0)]
        public float Latitude { get; set; }
        [ColumnName("Longitude"), LoadColumn(1)]
        public float Longitude { get; set; }

    }

    public class GeolocationOutput
    {
        [ColumnName("PredictedLabel")]
        public uint PredictedClusterId { get; set; }
        [ColumnName("Score")]
        public float[] Distances { get; set; }
    }
}
