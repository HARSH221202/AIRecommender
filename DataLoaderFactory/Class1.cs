using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Configuration;
using System.Data;

namespace AIRecommender.DataLoader
{
    public interface IDataLoaderFactory
    {
        IDataLoader GetDataLoader();
    }

    public class DataLoaderFactory : IDataLoaderFactory
    {
        protected DataLoaderFactory()
        {

        }

        public static readonly DataLoaderFactory Instance = new DataLoaderFactory();

        public virtual IDataLoader GetDataLoader()
        {
            string className = ConfigurationManager.AppSettings["Loader"];
            Console.WriteLine(className);
            Type type = Type.GetType(className);
            return (IDataLoader)Activator.CreateInstance(type);
        }
    }
}