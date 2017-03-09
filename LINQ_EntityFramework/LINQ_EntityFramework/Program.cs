using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Common;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LINQ_EntityFramework
{
    public class DbStatistics
    {
        private ISchoolDbContext4 _db;
        public DbStatistics(ISchoolDbContext4 db )
        {
            _db = db;
            
                PeopleCount = _db.People.Count();
                CarCount = _db.Cars.Count();
             // auto closed
        }
        public int PeopleCount { get; set; }
        public int CarCount { get; set; }

        public IEnumerable<Tuple<string, int>> CarsPerPerson
        {
            get
            {
                return _db.People
                //    .Include(p => p.Cars)
                    .ToList()
                    .Select(p => new {Name = p.Last, CarCount = p.Cars.Count(c=>c.Make != null) })
                    .ToList()
                    .Select(o => new Tuple<string, int>(o.Name, o.CarCount));
            }
        }
    }


    public enum Color { Unset = 0, Red, Indigo, Purple, Algerian, Green, Lavender}

    public class Car
    {
        public int CarId { get; set; }
        public Color Color { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }


        //knows by convention that this is a Foreign Key
        public int? PersonId { get; set; }

        //Navigation Property (by default lazy loads Person on access)..virtual required
        public virtual Person Person { get; set; }
    }

    // Person to Cars is 1 to many

    public class Person // POCO
    {
        public Person()
        {
            Cars = new List<Car>();
        }
        // no need for[Key] to identify the Primary
        public int PersonId { get; set; } // primary key // every entity MUST have a Primary Key
        public string First { get; set; }
        public string Last { get; set; }
        public string Suffix { get; set; }

        [NotMapped]
        public String FullName { get { return $"{Last}, {First}"; } }

        public List<Car> Cars { get; set; }
    }

    public interface ISchoolDbContext4
    {
        DbSet<Person> People { get; set; }
        DbSet<Car> Cars { get; set; }
    }

    public class SchoolDbContext4 : DbContext, ISchoolDbContext4
    {
        public SchoolDbContext4() { }

        public SchoolDbContext4(DbConnection dbConnection) : base(dbConnection, true)
        { // in Java super(dbConnection, true)
            
        }

        public DbSet<Person> People { get; set; }
        public DbSet<Car> Cars { get; set; }
    }

    class Program
    {
        static void AddPerson(Person p2)
        {
            Person p = new Person
            {
                First = "David",
                Last = "Langstein",
            };
            p.Cars = new List<Car>();
            p.Cars.Add(new Car
            {
                Color = Color.Algerian,
                Make = "Mazda",
                Model = "911"

            });
            using (var db = new SchoolDbContext4())
            {
                db.People.Add(p);
                db.SaveChanges();
            }
        }
        static void Main(string[] args)
        {
            //if (new String("Bob".ToCharArray()) == new String("Bob".ToCharArray()))
            //    Console.WriteLine("Really?");
                
            using (var db = new SchoolDbContext4())
            {
                //var david = db.People.Include(p=>p.Cars) //loads Cars despite lazy loading
                //    .FirstOrDefault(p => p.First.StartsWith("Dav"));
                //if (david == null)
                //{
                //    // log 
                //}
                //david.Cars.Add(new Car {Color = Color.Green, Make = "Ferrarri", Model = "912"});
                //david.First = "Dave"; // EF tracks changes and will save them

                var c = new Car() {Color = Color.Lavender};

                db.Cars.Add(c);

                db.SaveChanges();

               // Console.WriteLine(david);
            }
        }


        static void Main2(string[] args)
        {
            using (var db = new SchoolDbContext4()) 
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

                var AnonObjectList = db.People
                    .Select(p222 => new 
                    {
                        FullName = p222.Last + ", " + p222.First,
                        Suffix = p222.Suffix,
                        Useless = 5
                    }).ToList();

                foreach (var qqqq in AnonObjectList)
                {
                    Console.WriteLine(qqqq.FullName);
                }

                //var _3ers = db.People
                //    .Where(p => p.Suffix.ToCharArray().Length == 1);  // will fail since when LINQ query is realized it will be converted to SQL, and SQL does not support toCharArray
                //foreach (var p in _3ers)
                //{
                //    Console.WriteLine(p.Suffix);
                //}
                Console.ReadLine();
            }
        }
    }
}
