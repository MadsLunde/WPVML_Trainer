using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;

using Accord.Math;

using CsvHelper;

using Microsoft.ML;
using Microsoft.ML.Data;
using Microsoft.ML.Trainers;
using Microsoft.ML.Transforms;

using PLplot;

using WPVML_Trainer;

using WPVML_TrainerML.ConsoleApp.Helpers.ModelMappers;
using WPVML_TrainerML.Model.Models;

namespace WPVML_TrainerML.ConsoleApp.Trainers
{
    public class UserMatrixFactorization
    {

        private static string MODEL_FILEPATH = @"../../../../WPVML_TrainerML.Model/UserMatrix.zip";
        public static void CreateModel(int apprixmationRank, int numberOfIterations)
        {
            var mlcontext = new MLContext();
            var repo = new Repository();
            Console.WriteLine($"Receiving sessions for training from RavenDB...");
            var sessions = repo.GetAlotOfTrainingSessions();
            
            var dataset = new List<UserMatrix>();

            foreach (var session in sessions)
            {
                dataset.AddRange(UserMatrixMapper.Map(session));
            }

            Console.WriteLine($"Sessions collected: {dataset.Count}");

            Console.WriteLine();
            Console.WriteLine("Press any key to continue");
            Console.ReadKey();

            Console.WriteLine(" =============== Initializing training... =============== ");
            //Plot(dataset);
            var evaluationSet = new List<UserMatrix>();

            for (int i = 0; i < dataset.Count*0.25; i++)
            {
                evaluationSet.Add(dataset[i]);
                dataset.RemoveAt(i);
            }

            IDataView trainingDataView = mlcontext.Data.LoadFromEnumerable(dataset);

            EstimatorChain<NormalizingTransformer> dataProcessingPipeline =
                mlcontext.Transforms.Conversion
                .MapValueToKey(outputColumnName: "UserIdEncoded",
                    inputColumnName: "UserId")
                .Append(mlcontext.Transforms.Conversion.
                    MapValueToKey(outputColumnName: "NodeIdEncoded",
                    inputColumnName: "NodeId"))
                .Append(mlcontext.Transforms
                    .NormalizeBinning(outputColumnName: "Label", 
                        inputColumnName: "TimeSpent", 
                        maximumBinCount: 3));

            MatrixFactorizationTrainer.Options options = 
                                    new MatrixFactorizationTrainer.Options();
            options.MatrixColumnIndexColumnName = "UserIdEncoded";
            options.MatrixRowIndexColumnName = "NodeIdEncoded";
            options.LabelColumnName = "Label";
            options.NumberOfIterations = numberOfIterations;
            options.ApproximationRank = apprixmationRank;

            var trainingPipeline = dataProcessingPipeline.
                Append(mlcontext.Recommendation().
                    Trainers.MatrixFactorization(options));
            
            using (TextWriter writer = new StreamWriter(@"../../../../WPVML_TrainerML.Model/transformedUserMatrixDataViewColumn.csv", false, System.Text.Encoding.UTF8))
            {
                
                var columnData = dataProcessingPipeline.Preview(trainingDataView, 9999).ColumnView;
                var rowData = dataProcessingPipeline.Preview(trainingDataView, 9999).RowView;
                var data = new List<EncodedUserMatrixForCsv>();
                foreach (var info in rowData)
                {
                    data.Add(new EncodedUserMatrixForCsv()
                    {
                        NodeId = (uint) info.Values[4].Value,
                        UserID = (uint) info.Values[3].Value,
                        Label = (float) info.Values[5].Value
                    });
                }
                var csv = new CsvWriter(writer);
                csv.WriteRecords(data); // where values implements IEnumerable
            }
            
            ITransformer model = trainingPipeline.Fit(trainingDataView);
            Console.WriteLine("Training complete");

            Console.WriteLine("Press any key to continue");
            Console.WriteLine();
            Console.ReadKey();

            Console.WriteLine("=============== Evaluating the model ===============");
            IDataView testDataView = mlcontext.Data.LoadFromEnumerable(evaluationSet);
            var prediction = model.Transform(testDataView);
            var metrics = mlcontext.Regression.Evaluate(prediction, labelColumnName: "Label", scoreColumnName: "Score");

            Console.WriteLine($"Loss Function: {metrics.LossFunction}");
            Console.WriteLine($"Mean Absolute Error: {metrics.MeanAbsoluteError}");
            Console.WriteLine($"Mean Squared Error: {metrics.MeanSquaredError}");
            Console.WriteLine($"Root Mean Squared Error: {metrics.RootMeanSquaredError}");



            SaveModel(mlcontext, model, MODEL_FILEPATH, trainingDataView.Schema);
        }

        private static void SaveModel(MLContext mlContext, ITransformer mlModel, string modelRelativePath, DataViewSchema modelInputSchema)
        {
            // Save/persist the trained model to a .ZIP file
            Console.WriteLine();
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

    }
}


