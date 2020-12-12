using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
//---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
using Accord.MachineLearning.Rules;

using WPVML_Trainer.Models;

namespace WPVML_TrainerML.Model.Accord
{
    public class PageVisitAssociation
    {
        private Apriori apriori;
        public PageVisitAssociation()
        {
             apriori = new Apriori(threshold: 1, confidence: 0.05);
        }

        private string[] accpetedDocTypes =
        {
            "sectionPage",
            "spotPage",
            "newsPage"
        };

        private string[] bannedDocTypes =
        {
            "categoryPage",
            "productPage",
            "shopPage"
        };

        public AssociationRuleMatcher<int> TrainApriori(List<Session> sessions)
        {
            SortedSet<int>[] dataset = new SortedSet<int>[sessions.Count];
            for (int i = 0; i < sessions.Count; i++)
            {
                var row = new SortedSet<int>();
                foreach (var visit in sessions[i].PageVisits)
                {
                    
                    if (!bannedDocTypes.Contains(visit.DocumentTypeAlias))
                    {
                    
                        row.Add(visit.NodeId);    
                    }
                    
                }

                dataset[i] = row;
            }
            AssociationRuleMatcher<int> classifier = apriori.Learn(dataset);

            double totalScore = 0;
            double totalSupport = 0;
            double totalLift = 0;
            foreach (var rule in classifier.Rules)
            {
                totalScore += rule.Confidence;
                totalSupport += rule.Support;
                totalLift += rule.Confidence / classifier.Rules.Length;

            }
            Console.WriteLine($"Confidence; {totalScore/classifier.Rules.Length}");
            Console.WriteLine($"Support: {totalSupport/classifier.Rules.Length}");

            return classifier;

        }

        public int[][] MostProbablePages(AssociationRuleMatcher<int> classifier, List<PageVisit> visit)
        {
            var pages = visit.Select(x => x.NodeId).ToArray();
            int[][] matches = classifier.Decide(pages);
            

            return matches;
        }
    }
}
