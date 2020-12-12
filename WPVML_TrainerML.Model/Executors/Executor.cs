using Microsoft.ML;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

using WPVML_TrainerML.Model.Models;

namespace WPVML_TrainerML.Model.Executors
{
    public class Executor
    {
        public static MostImportantPageOutput PredictSdca(MostImportantPage input)
        {
            // Create new MLContext
            MLContext mlContext = new MLContext();

            // Load model & create prediction engine
            string modelPath = "../../../../WPVML_TrainerML.Model/SdcaMaximumEntropy.zip";
            ITransformer mlModel = mlContext.Model.Load(modelPath, out var modelInputSchema);
            var predEngine = mlContext.Model.CreatePredictionEngine<MostImportantPage, MostImportantPageOutput>(mlModel);

            // Use model to make prediction on input data
            MostImportantPageOutput result = predEngine.Predict(input);
            return result;
        }

        public static UserMatrixOutput PredictUserMatrix(UserMatrix input, PredictionEngine<UserMatrix, UserMatrixOutput> predictionengine)
        {
            var output = predictionengine.Predict(input);
         
            return output;

        }

        public static ExternalDataOutput PredictExternalDataClusters(ExternalData input)
        {
            MLContext mlContext = new MLContext();

            string modelPath = "../../../../WPVML_TrainerML.Model/ExternalDataKMeans.zip";
            ITransformer mlModel = mlContext.Model.Load(modelPath, out var modelInputSchema);

            var predictionEngine = mlContext.Model.CreatePredictionEngine<ExternalData, ExternalDataOutput>(mlModel);
            var output = predictionEngine.Predict(input);

            return output;
        }
    }
}
