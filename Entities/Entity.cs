using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class BookDetails
    {
        public List<Book> Books = new List<Book>();
        public List<BookUserRating> useratings = new List<BookUserRating>();
        public List<User> users = new List<User>();
    }
    public class Book
    {
        public string BookTitle { get; set; }
        public string BookAuthor { get; set; }
        public string ISBN { get; set; }
        public string Publisher { get; set; }
        public int YearOfPublication { get; set; }
        public string ImageurlSmall { get; set; }
        public string ImageurlMedium { get; set; }
        public string ImageurlLarge { get; set; }
    }
    public class BookUserRating
    {
        public int Rating { get; set; }
        public string ISBN { get; set; }
        public int UserId { get; set; }
    }
    public class User
    {
        public int UserId { get; set; }
        public int Age { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
    }
}
