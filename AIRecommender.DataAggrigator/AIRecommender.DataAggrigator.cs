using Entities;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace AIRecommender.DataAggrigator
{
    public class Preference
    {
        public string ISBN { get; set; }
        public string State { get; set; }
        public int Age { get; set; }
        public Preference(string isbn , string state, int age)
        {
            ISBN = isbn;
            State = state;
            Age = age;
        }
    }
    public interface IDataAaggrigator
    { 
        ConcurrentDictionary<string, List<int>> Aggrigate(BookDetails bookDetails, Preference preference);
    }
    public class RatingsAggrigator : IDataAaggrigator
    {
        public ConcurrentDictionary<string, List<int>> Aggrigate(BookDetails bookDetails, Preference preference)
        {
            ConcurrentDictionary<string, List<int>> aggrigate1 = new ConcurrentDictionary<string, List<int>>();
            object lockObject = new object();

            HashSet<int> prefferedUserIds = new HashSet<int>();
            //ParallelOptions parallelOptions = new ParallelOptions();
            //parallelOptions.MaxDegreeOfParallelism= 8;
            try
            {
                Parallel.ForEach(bookDetails.users, bookuser =>
                {
                    if (bookuser.Age == preference.Age || bookuser.State == preference.State)
                    {
                        lock (lockObject)
                        {
                            prefferedUserIds.Add(bookuser.UserId);
                        }
                    }
                });

                Parallel.ForEach(bookDetails.useratings, item =>
                {
                    if (prefferedUserIds.Contains(item.UserId))
                    {
                        lock (lockObject)
                        {
                            if (!aggrigate1.ContainsKey(item.ISBN))
                            {
                                aggrigate1[item.ISBN] = new List<int>();
                            }

                        aggrigate1[item.ISBN].Add(item.Rating);
                    }
                }
                    
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }

            return aggrigate1;
        }

        public string AgeGroup(int age)
        {
            if (age >= 1 && age <= 16)
            {
                return "Teen";
            }
            else if(age>=17&&age<= 30)
            {
                return "Young";
            }
            else if (age >= 31 && age <= 50)
            {
                return "Mid";
            }
            else if (age>=51 && age<= 60)
            {
                return "Old";
            }
            else
            {
                return "Senior";
            }
        }
    }
}
