using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using Microsoft.ML;

using WPVML_Trainer;

using WPVML_TrainerML.ConsoleApp.Helpers;
using WPVML_TrainerML.Model.Models;

namespace WPVML_TrainerML.ConsoleApp.Trainers
{
    public class KMeans
    {

        private static string MODEL_FILEPATH = @"../../../../WPVML_TrainerML.Model/ExternalDataKMeans.zip";

        public static void CreateModel(int k = 5)
        {
            var mlcontext = new MLContext();
            var repo = new Repository();
            var sessions = repo.GetTrainingSessions();
            var data = new List<ExternalData>();
            foreach (var session in sessions)
            {
                data.Add(new ExternalData()
                {
                    Rain = Convert.ToSingle(session.Weather.Rain),
                    Temperature = (float) session.Weather.Temperature,
                    Day = session.DateTime.Day,
                    WindSpeed = (float) session.Weather.WindSpeed,
                    Hour = session.DateTime.Hour,
                    Cloudiness = session.Weather.Cloudiness
                });
            }

            IDataView dataView = mlcontext.Data.LoadFromEnumerable(data);

            var pipeline = mlcontext.Transforms
                .Concatenate("Features", "Hour", "Day", "Temperature", "WindSpeed", "Rain", "Cloudiness")
                .Append(mlcontext.Clustering.Trainers.KMeans("Features", numberOfClusters: k));

            var model = pipeline.Fit(dataView);

            SaveModel(mlcontext, model, MODEL_FILEPATH, dataView.Schema);
        }

        private static void SaveModel(MLContext mlContext, ITransformer mlModel, string modelRelativePath, DataViewSchema modelInputSchema)
        {
            // Save/persist the trained model to a .ZIP file
            Console.WriteLine($"=============== Saving the model  ===============");
            mlContext.Model.Save(mlModel, modelInputSchema, GetAbsolutePath(modelRelativePath));
            Console.WriteLine("The model is saved to {0}", GetAbsolutePath(modelRelativePath));
        }

        public static string GetAbsolutePath(string relativePath)
        {
            FileInfo _dataRoot = new FileInfo(typeof(Program).Assembly.Location);
            string assemblyFolderPath = _dataRoot.Directory.FullName;

            string fullPath = Path.Combine(assemblyFolderPath, relativePath);

            return fullPath;
        }

        public static void calculateWSS(List<ExternalData> data, int kmax)
        {
            var repo = new Repository();
            MLContext mlContext = new MLContext();
            List<float> sse = new List<float>();
            for (int k = 1; k <= kmax; k++)
            {
                CreateModel(k);

                string modelPath = "../../../../WPVML_TrainerML.Model/ExternalDataKMeans.zip";
                ITransformer mlModel = mlContext.Model.Load(modelPath, out var modelInputSchema);


                var predictionEngine = mlContext.Model.CreatePredictionEngine<ExternalData, ExternalDataOutput>(mlModel);
                float curr_sse = 0;
                foreach (var externalData in data)
                {
                    var output = predictionEngine.Predict(externalData);
                    curr_sse += output.Distances[output.PredictedClusterId-1];
                }

                sse.Add(curr_sse);
            }

            MappedSessionsToCsv.WriteKmeansResultsToCsv(sse);
        }
    }
}
