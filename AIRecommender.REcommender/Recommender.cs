using AIRecommender.BookDataService;
using AIRecommender.Cache;
using AIRecommender.CoreEngine;
using AIRecommender.DataAggrigator;
using AIRecommender.DataLoader;
using Entities;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIRecommender.REcommender
{
    public class AIRecommendationEngine
    {
        BookDataServices bookDataService;
        private BookDetails bookDetails;

        public AIRecommendationEngine(BookDataServices bookDataService)
        {
            this.bookDataService = bookDataService;
            this.bookDetails = bookDataService.GetBookDetails();
        }
        

        IDataAaggrigator aggrigator = new RatingsAggrigator();
        IRecommender recommender = new PearsonRecommender();

        public Dictionary<string, string> Recommend(Preference preference, int limit)
        {
            ConcurrentDictionary<string, List<int>> aggrigate = aggrigator.Aggrigate(bookDetails, preference);

            Dictionary<string, double> ints = new Dictionary<string, double>();

            if (aggrigate.ContainsKey(preference.ISBN))
            {
                foreach (var e in aggrigate)
                {
                    double a = recommender.Getcorrelation(aggrigate[preference.ISBN].ToArray(), e.Value.ToArray());
                    ints.Add(e.Key, a);
                }
            }
            else
            {
                Console.WriteLine($"No recommendation found for ISBN: {preference.ISBN}");
                return new Dictionary<string, string>();
            }

            var sortedRecommendations = ints
             .OrderByDescending(x => x.Value)
             .Take(limit)
             .ToList();

            Dictionary<string, string> list = new Dictionary<string, string>();

            foreach (var book1 in sortedRecommendations)
            {
                string title = GetBookTitleByISBN(book1.Key, bookDetails);
                list.Add(book1.Key, title);
            }

            return list;
        }
        public string GetBookTitleByISBN(string isbn, BookDetails bookDetails)
        {
            foreach (var book in bookDetails.Books)
            {
                if (book.ISBN == isbn)
                {
                    return book.BookTitle;
                }
            }
            return "Unknown Title";
        }

    }
}
