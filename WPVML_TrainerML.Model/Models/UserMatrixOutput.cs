using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.ML.Data;

namespace WPVML_TrainerML.Model.Models
{
    public class UserMatrixOutput
    {
        [ColumnName("PredictedLabel")]
        public float Prediction { get; set; }
        public float Score { get; set; }
    }
}
