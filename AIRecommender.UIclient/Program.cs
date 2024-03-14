using AIRecommender.BookDataService;
using AIRecommender.Cache;
using AIRecommender.CoreEngine;
using AIRecommender.DataAggrigator;
using AIRecommender.DataLoader;
using AIRecommender.REcommender;
using Entities;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace AIRecommender.UIclient
{
    public class Users
    {
        static void Main(string[] args)
        {
            //IDataCacher dataCacher = new RedisCacheService(ConfigurationManager.AppSettings["RedisCacheKey"]);
            //BookDataServices bookDataService = new BookDataServices(dataCacher);
            //AIRecommendationEngine aIRecommendationEngine = new AIRecommendationEngine(bookDataService);
            do
            {
                Console.WriteLine("ISBN of the book to order: ");
                string isbn = Console.ReadLine();
                Console.WriteLine("State of the user: ");
                string state = Console.ReadLine();
                Console.WriteLine("Age of the user: ");
                int age = int.Parse(Console.ReadLine());
                Console.WriteLine("The Recommended books are: ");
                Stopwatch sw = Stopwatch.StartNew();
                IDataCacher dataCacher = new RedisCacheService(ConfigurationManager.AppSettings["RedisCacheKey"]);
                BookDataServices bookDataService = new BookDataServices(dataCacher);
                AIRecommendationEngine aIRecommendationEngine = new AIRecommendationEngine(bookDataService);
                Dictionary<string, string> books = aIRecommendationEngine.Recommend(new Preference(isbn, state, age), 10);
                foreach (var item in books)
                {
                    Console.WriteLine(item.Key + "  " + item.Value);
                }
                Console.WriteLine("Time taken : " + sw.ElapsedMilliseconds+"ms");
                Console.WriteLine("----------------------");
                Console.WriteLine("New Order : (y/n)");
            } while (Console.ReadLine().ToLower() == "y");
        }
    }
}
