
using AIRecommender.Cache;
using AIRecommender.DataLoader;
using Entities;
using System;
using System.Configuration;

namespace AIRecommender.BookDataService
{
    public class BookDataServices
    {
        private readonly IDataLoaderFactory dataLoaderFactory = DataLoaderFactory.Instance;
        IDataCacher dataCacher; /*= new RedisCacheService("BookDetailsCacheKey");*/

        public BookDataServices(IDataCacher dataCacher)
        {
            this.dataCacher = dataCacher;
        }
        public BookDetails GetBookDetails()
        {
            BookDetails cachedData = dataCacher.Get();
            if (cachedData != null)
            {
                return cachedData;
            }
            IDataLoader dataLoader = dataLoaderFactory.GetDataLoader();
            BookDetails bookDetails = dataLoader.Load();

            dataCacher.Set(bookDetails);

            return bookDetails;
        }
    }

}
