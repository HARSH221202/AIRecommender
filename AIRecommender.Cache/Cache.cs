using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace AIRecommender.Cache
{
    public interface IDataCacher
    {
        BookDetails Get();
        void Set(BookDetails value);
    }
    public class RedisCacheService : IDataCacher
    {
         IDatabase redisDatabase;
         private readonly string cacheKey;

        public RedisCacheService(string cacheKey)
        {
            this.cacheKey = cacheKey;

            ConnectionMultiplexer redisConnection = ConnectionMultiplexer.Connect("localhost:6379");
            redisDatabase = redisConnection.GetDatabase();
        }

        public BookDetails Get()
        {
            string cachedData = redisDatabase.StringGet(cacheKey);
            //if (!string.IsNullOrEmpty(cachedData))
            //{
            //    Console.WriteLine("Cache hit for " + cacheKey);
            //}
            return string.IsNullOrEmpty(cachedData) ? null : JsonConvert.DeserializeObject<BookDetails>(cachedData);
        }

        public void Set(BookDetails value)
        {
            redisDatabase.StringSet(cacheKey, JsonConvert.SerializeObject(value));
        }
    }

    public class MemDataCacher : IDataCacher
    {
        public BookDetails BookDetails { get; set; }

        public BookDetails Get()
        {
            return BookDetails;
        }

        public void Set(BookDetails bookDetails)
        {
            BookDetails = bookDetails;

        }
    }
}
