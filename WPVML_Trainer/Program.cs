using System;
using System.Collections.Generic;
using System.Linq;

using WPVML_Trainer.Models;

using WPVML_TrainerML.Model.Accord;

namespace WPVML_Trainer
{
    class Program
    {
        static void TestApriori(int numberOfSessions)
        {
            string[] bannedDocTypes =
            {
                "categoryPage",
                "productPage",
                "shopPage"
            };


            Repository repo = new Repository();
            var sessionsRaw = repo.GetTrainingSessions();
            PageVisitAssociation aprirori = new PageVisitAssociation();
            var sessions = sessionsRaw.Take(numberOfSessions).ToList();
            foreach (var session in sessions)
            {
                List<PageVisit> visitsToRemove = new List<PageVisit>();
                foreach (var sessionPageVisit in session.PageVisits)
                {
                    if (bannedDocTypes.Contains(sessionPageVisit.DocumentTypeAlias))
                    {
                        visitsToRemove.Add(sessionPageVisit);
                    }
                }

                foreach (var pageVisit in visitsToRemove)
                {
                    session.PageVisits.Remove(pageVisit);
                }
            }

            var rules = aprirori.TrainApriori(sessions);
            var r = new Random().Next(0, sessions.Count);
            
            var rPage = sessions[r].PageVisits;
            var matches = aprirori.MostProbablePages(rules, rPage);
        }

        static void Main(string[] args)
        {
            /*
            Console.WriteLine("100 sessions");
            TestApriori(100);

            Console.WriteLine("200 sessions");
            TestApriori(200);

            Console.WriteLine("300 sessions");
            TestApriori(300);

            Console.WriteLine("400 sessions");
            TestApriori(400);

            Console.WriteLine("500 sessions");
            TestApriori(600);
            */
            Console.WriteLine("700 sessions");
            TestApriori(700);
        }
    }
}
