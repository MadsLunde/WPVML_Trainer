using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.ML.Data;

namespace WPVML_TrainerML.Model.Models
{
    public class UserMatrix
    {
        [ColumnName("UserId"), LoadColumn(0)]
        public float UserId { get; set; }
        [ColumnName("NodeId"), LoadColumn(1)]
        public float NodeId { get; set; }
        [ColumnName("TimeSpent"), LoadColumn(2)]
        public float TimeSpent { get; set; }
        public void Print()
        {
            Console.WriteLine("User Matrix model...\n\n");
            Console.WriteLine($"UserId: {this.UserId}");
            Console.WriteLine($"NodeId: {this.NodeId}");
            Console.WriteLine($"TimeSpent: {this.TimeSpent}");
        }
    }
}
