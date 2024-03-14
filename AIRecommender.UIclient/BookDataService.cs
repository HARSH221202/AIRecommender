namespace AIRecommender.UIclient
{
    internal class BookDataService
    {
        private object dataLoaderFactory;
        private object dataCacher;

        public BookDataService(object dataLoaderFactory, object dataCacher)
        {
            this.dataLoaderFactory = dataLoaderFactory;
            this.dataCacher = dataCacher;
        }
    }
}