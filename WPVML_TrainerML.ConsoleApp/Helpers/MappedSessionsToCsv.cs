using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using CsvHelper;

using WPVML_Trainer;

using WPVML_TrainerML.ConsoleApp.Helpers.ModelMappers;
using WPVML_TrainerML.Model;
using WPVML_TrainerML.Model.Models;

namespace WPVML_TrainerML.ConsoleApp.Helpers
{
    public class MappedSessionsToCsv
    {
        public static void WriteCsv()
        {
            var mappedList = new List<UserMatrix>();
            var repo = new Repository();
            var sessions = repo.GetTrainingSessions();

            foreach (var session in sessions)
            {
                mappedList.AddRange(UserMatrixMapper.Map(session));
            }

            using (TextWriter writer = new StreamWriter(@"../../../../WPVML_TrainerML.Model/sessions.csv", false, System.Text.Encoding.UTF8))
            {
                var csv = new CsvWriter(writer);
                csv.WriteRecords(mappedList); // where values implements IEnumerable
            }
        }

        public static void WriteKmeansResultsToCsv(List<float> sse)
        {
            using (TextWriter writer = new StreamWriter(@"../../../../WPVML_TrainerML.Model/kmeansResults.csv", false, System.Text.Encoding.UTF8))
            {
                var csv = new CsvWriter(writer);
                csv.WriteRecords(sse); // where values implements IEnumerable
            }
        }
    }
}
