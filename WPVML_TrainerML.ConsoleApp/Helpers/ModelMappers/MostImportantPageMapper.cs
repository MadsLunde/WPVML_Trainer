using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WPVML_Trainer.Models;
using WPVML_TrainerML.Model;

namespace WPVML_TrainerML.ConsoleApp.Helpers.ModelMappers
{
    static class MostImportantPageMapper
    {
        private static string[] bannedDocTypes =
        {
            "categoryPage",
            "productPage",
            "shopPage"
        };

        private static int[] bannedNodeIds =
        {
            1334,
            1331
        };

        public static MostImportantPage Map(Session session)
        {
            int mostTime = 0;
            int mostImportantPage = 0;
            if (session.PageVisits != null && session.PageVisits.Count > 0)
            {
                foreach (var visit in session.PageVisits)
                {
                    if (visit.TimeSpent > mostTime && !bannedDocTypes.Contains(visit.DocumentTypeAlias) && !bannedNodeIds.Contains(visit.NodeId))
                    {
                        mostImportantPage = visit.NodeId;
                    }
                }
            }

            return new MostImportantPage()
            {
                NodeId = mostImportantPage,
                Day = session.DateTime.Day,
                Hour = session.DateTime.Hour,
                LocationLatitude = session.Location.Latitude,
                LocationLongitude = session.Location.Longitude,
                Temperature = session.Weather.Temperature
            };
        }

        
    }
}
