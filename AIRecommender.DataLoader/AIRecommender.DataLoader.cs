using Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using StackExchange.Redis;
using AIRecommender.Cache;

namespace AIRecommender.DataLoader
{

    public interface IDataLoader
    {
        BookDetails Load();
    }

    public class CSVDataLoader : IDataLoader
    {
        public BookDetails Load()
        { 
             BookDetails bookDetails = new BookDetails();

            List<User> users = LoadUsersParallel("BX-Users.csv");
            bookDetails.users = users;

            List<Book> books = LoadBooks("BX-Books.csv");
            bookDetails.Books = books;

           List<BookUserRating> bookUserRatings = LoadBookUserRatings("BX-Book-Ratings.csv");
            bookDetails.useratings = bookUserRatings;

            return bookDetails;
        }
        private List<User> LoadUsersParallel(string filePath)
        {
            List<User> users = new List<User>();
            object lockObject = new object();

            try
            {
                Parallel.ForEach(File.ReadLines(filePath), item =>
                {
                    try
                    {
                        string[] fields = item.Split(';').Select(s => s.Trim('"')).ToArray();

                        string userId = fields[0];
                        int age = 0;
                        string age1 = fields[2];
                        if (age1 == "NULL")
                        {
                            age = 0;
                        }
                        else
                        {
                            age = int.Parse(age1);
                        }

                        string[] location = fields[1].Split(',').Select(s => s.Trim()).ToArray();

                        User user = new User
                        {
                            UserId = int.Parse(userId),
                            Age = age,
                            City = location[0],
                            State = location[1],
                            Country = location[2]
                        };

                        lock (lockObject)
                        {
                            users.Add(user);
                        }

                    }
                    catch (Exception ex)
                    {
                        
                    }
                });
            }
            catch (Exception ex)
            {
                
            }

            return users;
        }
        private List<Book> LoadBooks(string filePath)
        {
            List<Book> books = new List<Book>();
            object lockObject = new object();

            try
            {
                Parallel.ForEach(File.ReadLines(filePath), item =>
                {
                    try
                    {
                        //string delimiter = "\";\"";
                        item = item.Trim('"');
                        string[] fields = item.Split(';');

                        //for (int i = 0; i < fields.Length; i++)
                        //{
                        //    fields[i] = fields[i].Trim('"');
                        //}

                        int yop = int.Parse(fields[3]);

                        Book book = new Book
                        {
                            ISBN = fields[0],
                            BookTitle = fields[1],
                            BookAuthor = fields[2],
                            YearOfPublication = yop,
                            Publisher = fields[4],
                            ImageurlSmall = fields[5],
                            ImageurlMedium = fields[6],
                            ImageurlLarge = fields[7]
                        };

                        lock (lockObject)
                        {
                            books.Add(book);
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error loading book data: {ex.Message}");
                    }
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error reading book data: {ex.Message}");
            }

            return books;
        }



        private List<BookUserRating> LoadBookUserRatings(string filePath)
        {
            List<BookUserRating> bookUserRatings = new List<BookUserRating>();
            object lc = new object();
            try
            {
                foreach (string line in File.ReadLines(filePath))
                {
                    string[] fields = line.Split(';').Select(part => part.Trim('"')).ToArray();

                    BookUserRating rating = new BookUserRating
                    {
                        Rating = int.Parse(fields[2]),
                        ISBN = fields[1],
                        UserId = int.Parse(fields[0])
                    };
                    lock (lc)
                    {
                        bookUserRatings.Add(rating);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading book user ratings: {ex.Message}");
            }

            return bookUserRatings;
        }
    }
}
