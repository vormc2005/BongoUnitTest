using Bongo.DataAccess.Repository;
using Bongo.Models.Model;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bongo.DataAccess.Tests
{
    [TestFixture]
    public class StudyRoomBookingRepositoryTests
    {
        private StudyRoomBooking studyRoomBooking_One;
        private StudyRoomBooking studyRoomBooking_Two;
        private DbContextOptions<ApplicationDbContext> options;

        [SetUp]

        public void SetUp()
        {
            options = new DbContextOptionsBuilder<ApplicationDbContext>()
                 .UseInMemoryDatabase(databaseName: "temp_Bongo").Options;
        }

        public StudyRoomBookingRepositoryTests()
        {
            studyRoomBooking_One= new StudyRoomBooking() 
            {
                FirstName = "Ben1",
                LastName = "Stark1",
                Date = new DateTime(2023, 1,1),
                Email = "ben1@gmail.com",
                BookingId= 11,
                StudyRoomId= 1,
            };

            studyRoomBooking_Two = new StudyRoomBooking()
            {
                FirstName = "Ben2",
                LastName = "Stark2",
                Date = new DateTime(2023, 2, 2),
                Email = "ben2@gmail.com",
                BookingId = 22,
                StudyRoomId = 2,
            };
        }


        [Test]
        [Order(1)]
        public void SaveBooking_Booking_One_CheckTheValuesFromDatabase()
        {
            //arrange
            //var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            //    .UseInMemoryDatabase(databaseName: "temp_Bongo").Options;
            //act
            using(var context = new ApplicationDbContext(options))
            {
                var repository = new StudyRoomBookingRepository(context);
                repository.Book(studyRoomBooking_One);
            }
            //assert
            using (var context = new ApplicationDbContext(options))
            {
                var bookingFromDb = context.StudyRoomBookings.FirstOrDefault(u=>u.BookingId == 11);
                Assert.That(studyRoomBooking_One.BookingId, Is.EqualTo(bookingFromDb.BookingId));
                Assert.That(studyRoomBooking_One.FirstName, Is.EqualTo(bookingFromDb.FirstName));
                Assert.That(studyRoomBooking_One.LastName, Is.EqualTo(bookingFromDb.LastName));                
            }
        }

        [Test]
        [Order(2)]
        public void SaveBooking_Booking_One_AndTwo_CheckBothBookingsValuesFromDatabase()
        {
            //arrange
            var expectedResult = new List<StudyRoomBooking> { studyRoomBooking_One, studyRoomBooking_Two };

            //var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            //    .UseInMemoryDatabase(databaseName: "temp_Bongo").Options;
            using (var context = new ApplicationDbContext(options))
            {
                context.Database.EnsureDeleted();
                var repository = new StudyRoomBookingRepository(context);
                repository.Book(studyRoomBooking_One);
                repository.Book(studyRoomBooking_Two);
            }
            //act
            List<StudyRoomBooking> actualList;
            using (var context = new ApplicationDbContext(options))
            {
                var repository = new StudyRoomBookingRepository(context);
               actualList = repository.GetAll(null).ToList();
            }
            //assert
            CollectionAssert.AreEqual(expectedResult, actualList, new BookingCompare());
        }

        private class BookingCompare : IComparer
        {
            public int Compare(object x, object y)
            {
                var booking1 = (StudyRoomBooking)x;
                var booking2 = (StudyRoomBooking)y;
                if(booking1.BookingId != booking2.BookingId)
                {
                    return 1;
                }
                else
                {
                    return 0;
                }
            }
        }
    }
}
