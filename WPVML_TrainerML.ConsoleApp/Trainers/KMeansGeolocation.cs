using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.ML;

using WPVML_Trainer;

using WPVML_TrainerML.Model.Models;

namespace WPVML_TrainerML.ConsoleApp.Trainers
{
    public class KMeansGeolocation
    {
        private static string MODEL_FILEPATH = @"../../../../WPVML_TrainerML.Model/GeolocationClusteringModel.zip";
        public static void CreateModel()
        {
            var mlcontext = new MLContext();
            var repo = new Repository();
            var sessions = repo.GetTrainingSessions();
            var dataset = new List<Geolocation>();

            foreach (var session in sessions)
            {
                dataset.Add(new Geolocation()
                {
                    Longitude = (float)session.Location.Longitude,
                    Latitude = (float)session.Location.Latitude
                });
            }

            //var pipeline = mlcontext.Transforms.
        }
    }
}
