using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

using WPVML_Trainer.Models;

using WPVML_TrainerML.Model.Models;

namespace WPVML_TrainerML.ConsoleApp.Helpers.ModelMappers
{
    public class UserMatrixMapper
    {
        private static string[] bannedDocTypes =
        {
            "categoryPage",
            "productPage",
            "shopPage",
            "loginPage"
        };

        private static int[] bannedNodeIds =
        {
            1334,
            1331
        };

        public static List<UserMatrix> Map(Session session)
        {
            var model = new List<UserMatrix>();
            if (session.PageVisits != null && session.PageVisits.Count > 0)
            {
                foreach (var pageVisit in session.PageVisits)
                {
                    
                    if (!bannedDocTypes.Contains(pageVisit.DocumentTypeAlias) && !bannedNodeIds.Contains(pageVisit.NodeId))
                    {
                    
                        model.Add(new UserMatrix()
                        {
                            NodeId = pageVisit.NodeId,
                            TimeSpent = pageVisit.TimeSpent,
                            //DocType = pageVisit.DocumentTypeAlias,
                            UserId = int.Parse(Regex.Match(session.User, @"\d+").Value)
                        });
                        
                    }
                    
                    
                }
            }
            

            return model;
        }

        public static double StandardDeviation(IEnumerable<float> values)
        {
            double mean = values.Sum() / values.Count();

            var squaresQuery = values.Select(x => (x - mean) * (x - mean));
            var sumOfSquares = squaresQuery.Sum();

            return sumOfSquares / values.Count();
        }
    }
}
