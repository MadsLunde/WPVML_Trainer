using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CsvHelper;
using Raven.Client;
using Raven.Client.Indexes;
using WPVML_Trainer.Models;

namespace WPVML_Trainer
{
    public class Repository
    {
        public User GetUser(string userId)
        {
            using (IDocumentSession documentSession = DocumentStoreSingleton.Instance.GetStore("CeramicSpeedPersonalizationModule").OpenSession())
            {
                User user = documentSession
                    .Query<User>()
                    .Where(x => x.Id == userId)
                    .FirstOrDefault();
                return user;
            }
            
        }

        public List<PageVisit> GetPageVisitsForSessions(string sessionId)
        {
            using (IDocumentSession documentSession = DocumentStoreSingleton.Instance.GetStore("CeramicSpeedPersonalizationModule").OpenSession())
            {
                List<PageVisit> visits = documentSession.Load<Session>(sessionId).PageVisits;

                return visits;
            }
        }

        public List<Session> GetTrainingSessions()
        {
            using (IDocumentSession documentSession = DocumentStoreSingleton.Instance.GetStore("CeramicSpeedPersonalizationModule").OpenSession())
            {
                return documentSession.Query<Session>().Take(8000).Where(x => x.PageVisits.Count > 1).ToList();

            }
        }

        public List<Session> GetAlotOfTrainingSessions()
        {
            using (IDocumentSession documentSession = DocumentStoreSingleton.Instance.GetStore("CeramicSpeedPersonalizationModule").OpenSession())
            {
                //RavenQueryStatistics stats;
                //var points = new List<Session>();
                //var nextGroupOfPoints = new List<Session>();
                //const int ElementTakeCount = 1024;
                //int i = 0;
                //int skipResults = 0;

                //do
                //{
                //    nextGroupOfPoints = documentSession.Query<Session>().Statistics(out stats).Where(x => x.PageVisits.Count > 1).Skip(i * ElementTakeCount + skipResults).Take(ElementTakeCount).ToList();
                //    i++;
                //    skipResults += stats.SkippedResults;

                //    points = points.Concat(nextGroupOfPoints).ToList();
                //}
                //while (nextGroupOfPoints.Count == ElementTakeCount);

                //return points;

                //var query = documentSession.Query<Session>("PageVisits").Where(x => x.PageVisits.Count > 1);
                var mappedList = new List<Session>();

                using(var enumerator = documentSession.Advanced.Stream<Session>(startsWith: "sessions/", start: 0, pageSize: int.MaxValue))
                {
                    while(enumerator.MoveNext())
                    {
                        if(enumerator.Current.Document.PageVisits != null && enumerator.Current.Document.PageVisits.Count > 1)
                        {
                            mappedList.Add(enumerator.Current.Document);
                        }
                        
                    }
                    return mappedList;
                }
            }
        }



        public class SessionIndex: AbstractIndexCreationTask<Session>
        {
            public SessionIndex()
            {
                this.Map = sessions =>
                    from s in sessions
                    from p in s.PageVisits
                    select new
                    {
                        s.User,
                        p.NodeId
                    };
            }
        }

        public Session GetSampleSession()
        {
            using (IDocumentSession documentSession = DocumentStoreSingleton.Instance.GetStore("CeramicSpeedPersonalizationModule").OpenSession())
            {
                return documentSession.Load<Session>("sessions/706");
            }
        }

        public Session GetSampleSessionWithPageVisits()
        {
            using (IDocumentSession documentSession = DocumentStoreSingleton.Instance.GetStore("CeramicSpeedPersonalizationModule").OpenSession())
            {
                return documentSession.Query<Session>().Take(1).Where(x => x.PageVisits.Count > 1).FirstOrDefault();
            }
        }

        public List<User> GetAllusers()
        {
            using (IDocumentSession documentSession = DocumentStoreSingleton.Instance.GetStore("CeramicSpeedPersonalizationModule").OpenSession())
            {
                return documentSession.Query<User>().Take(4096).ToList();
            }
        }

        public List<Session> GetSessionsForUser(string user)
        {
            using (IDocumentSession documentSession = DocumentStoreSingleton.Instance.GetStore("CeramicSpeedPersonalizationModule").OpenSession())
            {
                return documentSession.Query<Session>().Where(x => x.User == user).ToList();
            }
        }

    }
}
