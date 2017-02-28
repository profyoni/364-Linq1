using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LINQ_EntityFramework
{
    public class Person // POCO
    {
        public int PersonId { get; set; } // primary key
        public string First { get; set; }
        public string Last { get; set; }
        public string Suffix { get; set; }
    }

    public class SchoolDbContext : DbContext
    {
        public DbSet<Person> People { get; set; }
    }

    class Program
    {
        static void Main(string[] args)
        {
            using (var db = new SchoolDbContext()) 
            {
                Console.WriteLine(db.People.Count()); // on first access will create database
                //for (int i = 0; i < 10; i++)
                //{
                //    var p = new Person
                //    {
                //        First = "Abe",
                //        Last = "Lincoln",
                //        Suffix = i + 1 + ""
                //    };
                //    db.People.Add(p);
                //}
                //db.SaveChanges();
                Console.WriteLine(db.People.Count()); // on first access will create database

                var AnonObkectList = db.People
                    .Select(p => new
                    {
                        FullName = p.Last + ", " + p.First,
                        Suffix = p.Suffix,
                        Useless = 5
                    }).ToList();
                foreach (var p in AnonObkectList)
                {
                    Console.WriteLine(p);
                }

                //var _3ers = db.People
                //    .Where(p => p.Suffix.ToCharArray().Length == 1);
                //foreach (var p in _3ers)
                //{
                //    Console.WriteLine(p.Suffix);
                //}
                Console.ReadLine();
            }
        }
    }
}
