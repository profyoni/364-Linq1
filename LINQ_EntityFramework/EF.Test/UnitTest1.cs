using System;
using System.Linq;
using FluentAssertions;
using LINQ_EntityFramework;
using NUnit.Framework;


namespace EF.Test
{

    public class UnitTest1
    {
        private SchoolDbContext4 _db;
        private DbStatistics dbStats;

        [SetUp]
        public void Init()
        {
            // in memory database - -isolated instances of the db
            _db = new SchoolDbContext4(Effort.DbConnectionFactory.CreateTransient());
            dbStats = new LINQ_EntityFramework.DbStatistics(_db);
        }

        [Test]
        public void TestMethod1()
        {
            _db.People.Add(new Person() {First = "Rafi", Last = "Gal"});
            _db.SaveChanges();
            _db.People.Count().Should().Be(1);

            dbStats.CarCount.Should().Be(0);
        }


        [Test]
        public void TestMethod2()
        {
            _db.People.Add(new Person() { First = "Eliezer", Last = "Hashimi" });
            _db.SaveChanges();
            _db.People.Count().Should().Be(1);

            dbStats.CarCount.Should().Be(0);
        }


        [Test]
        public void TestMethod3()
        {
            Seed(10);
            _db.People.Count().Should().Be(10);
            _db.Cars.Count().Should().Be(45);

            // number of cars per person: Last Name, Cars Per Person

            var carsPerPerson = dbStats.CarsPerPerson;

            carsPerPerson.Count().Should().Be(10);

            carsPerPerson.Last().Item2.Should().Be(9);
        }

        private void Seed(int numPersons)
        {
            for (int i = 0; i < numPersons; i++)
            {
                Person p;
                _db.People.Add(
                    p = new Person()
                {
                    First = i + "First",
                    Last = i + "Last",
                });
                for (int j = 0; j < i; j++)
                {

                    p.Cars.Add( new Car()
                    {
                       // Color = Enum.GetValues(typeof(Color))[i % Enum.GetValues(typeof(Color)).Length]),
                       Make = "Make" + j
                    });
                }
                _db.SaveChanges();
            }
        }
    }
}
